using System;
using System.Collections.Generic;
using System.Linq;

public class Pathfinder
{
    private const int HeuristicWeight = 1;
    private const int ActualWeight = 1;

    public int Level { get; }
    public int Width { get; }
    
    /// <summary>
    /// A map of the rooms on a level, given by the WizardLevel
    /// </summary>
    private readonly Room[,] _rooms;

    public Pathfinder(WizardLevel level)
    {
        Level = level.Level;
        Width = level.Width;
        _rooms = level.Rooms;
        _actual = new int[Width, Width, Width, Width];
        _state = new State[Width, Width, Width, Width];
        _open = new List<RoomId>[Width, Width];
        for (var x = 0; x < Width; x++)
        for (var z = 0; z < Width; z++)
            _open[x, z] = new List<RoomId>();
        _parent = new RoomId?[Width, Width, Width, Width];
        _path = new List<RoomId>[Width, Width, Width, Width];
    }

    /// <summary>
    /// Optimstic heuristic using simple unobstructed move count for value
    /// </summary>
    private int Heuristic(RoomId current, RoomId destination)
        => (Math.Abs(current.X - destination.X) + Math.Abs(current.Z - destination.Z)) * HeuristicWeight;

    /// <summary>
    /// Width by Width square grid for each origin room stored as [origin.X, origin.Z, current.X, current.Z]
    /// Contains move cost to reach a room from an origin room
    /// </summary>
    private readonly int[,,,] _actual;

    /// <summary>
    /// Searching state for a given room
    /// </summary>
    private enum State : byte
    {
        /// <summary>
        /// Room has not been considered yet
        /// </summary>
        Untouched,
        /// <summary>
        /// Room has been considered but not completed
        /// </summary>
        Open,
        /// <summary>
        /// Optimal path has been found for this room, use _actual and _parent to look it up
        /// </summary>
        Closed
    }

    /// <summary>
    /// Node state stored as [origin.X, origin.Z, current.X, current.Z]
    /// </summary>
    private readonly State[,,,] _state;

    /// <summary>
    /// A list of currently open rooms for each origin room, for faster iteration
    /// </summary>
    private readonly List<RoomId>[,] _open;

    /// <summary>
    /// A grid of parenting id's tracing the current room towards the origin
    /// A room will be null if it is untouched
    /// </summary>
    private readonly RoomId?[,,,] _parent;

    /// <summary>
    /// The room is marked as open
    /// As long as the destination indicated by taking the given direction from the previous room is accessible.
    /// If already open and a better path is found, updates the actual cost and parent
    /// </summary>
    /// <param name="origin">The origin room for the current path taken</param>
    /// <param name="previous">The room to take the given direction from</param>
    /// <param name="direction">Determines the destination room from the previous room</param>
    private void Open(RoomId origin, RoomId previous, Direction direction)
    {
        var current = previous[direction];
        if (!current.Bounds(Width) ||                                               // Out of bounds
            !_rooms[current.X, current.Z].FloorGen ||                               // Over a pit
            _rooms[previous.X, previous.Z].Walls[direction].Gen) return;            // Through a wall
        if (_state[origin.X, origin.Z, current.X, current.Z] == State.Untouched)    // Not visited before
        {
            _state[origin.X, origin.Z, current.X, current.Z] = State.Open;
            _open[origin.X, origin.Z].Add(current);
            _actual[origin.X, origin.Z, current.X, current.Z] =
                _actual[origin.X, origin.Z, previous.X, previous.Z] + ActualWeight;
            _parent[origin.X, origin.Z, current.X, current.Z] = previous; 
        }
        else if (_state[origin.X, origin.Z, current.X, current.Z] == State.Open &&  // Previously visited but still open
                 _actual[origin.X, origin.Z, current.X, current.Z] >                // Previous visit was worse
                 _actual[origin.X, origin.Z, previous.X, previous.Z] + ActualWeight)
        {
            // Update actual cost and parent with new path
            _actual[origin.X, origin.Z, current.X, current.Z] =
                _actual[origin.X, origin.Z, previous.X, previous.Z] + ActualWeight;
            _parent[origin.X, origin.Z, current.X, current.Z] = previous;
        }
    }

    /// <summary>
    /// Marks a room as closed and attempts to open any acessible rooms
    /// </summary>
    /// <param name="origin">The origin room for the current path taken</param>
    /// <param name="current">The room to close</param>
    private void Close(RoomId origin, RoomId current)
    {
        _state[origin.X, origin.Z, current.X, current.Z] = State.Closed;
        _open[origin.X, origin.Z].Remove(current);
        for (var d = (Direction) 0; d < (Direction) DirectionUtils.Count; d++)
            Open(origin, current, d);
    }

    /// <summary>
    /// A cache for previously found paths
    /// </summary>
    private readonly List<RoomId>[,,,] _path;

    public List<RoomId> Shortest(RoomId origin, RoomId destination)
    {
        // Check cache
        if (_path[origin.X, origin.Z, destination.X, destination.Z] != null)
            return _path[origin.X, origin.Z, destination.X, destination.Z];
        // Open origin room
        _state[origin.X, origin.Z, origin.X, origin.Z] = State.Open;
        _open[origin.X, origin.Z].Add(origin);
        // Proceed with A* by finding the lowest cost open room and closing it, functions take care of the rest
        while (_state[origin.X, origin.Z, destination.X, destination.Z] != State.Closed && _open.Length > 0)
        {
            var minRoomId = _open[origin.X, origin.Z].First();
            var minCost = _actual[origin.X, origin.Z, minRoomId.X, minRoomId.Z] + Heuristic(minRoomId, destination);
            foreach (var open in _open[origin.X, origin.Z])
            {
                var cost = _actual[origin.X, origin.Z, open.X, open.Z] + Heuristic(open, destination);
                if (cost > minCost)
                    continue;
                minRoomId = open;
                minCost = cost;
            }
            Close(origin, minRoomId);
        }
        // Build path list by parenting
        var path = new List<RoomId> {destination};
        var last = destination;
        for (var i = 0; i < _actual[origin.X, origin.Z, destination.X, destination.Z]; i++)
        {
            var roomId = _parent[origin.X, origin.Z, last.X, last.Z];
            if (roomId == null)
                break;
            last = (RoomId) roomId;
            path.Add(last);
        }
        _path[origin.X, origin.Z, destination.X, destination.Z] = path;
        return path;
    }
}
