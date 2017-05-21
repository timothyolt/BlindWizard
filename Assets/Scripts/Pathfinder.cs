using System;
using System.Collections.Generic;

public class Pathfinder
{
    private const int HeuristicWeight = 1;
    private const int ActualWeight = 1;

    public int Level { get; }
    public int Width { get; }
    private Room[,] _rooms;

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

    private void Open(RoomId origin, RoomId parent, Direction direction)
    {
        var current = parent.To(direction);
        if (_state[origin.X, origin.Z, current.X, current.Z] != State.Untouched || !current.Bounds(Width) /* or inacessible */) return;
        _state[origin.X, origin.Z, current.X, current.Z] = State.Open;
        _open[origin.X, origin.Z].Add(current);
        _actual[origin.X, origin.Z, current.X, current.Z] = _actual[origin.X, origin.Z, parent.X, parent.Z] + ActualWeight;
        _parent[origin.X, origin.Z, current.X, current.Z] = parent;
    }

    //TODO (timothyolt): Better paths should override

    private void Close(RoomId origin, RoomId current)
    {
        _state[origin.X, origin.Z, current.X, current.Z] = State.Closed;
        _open[origin.X, origin.Z].Remove(current);
        Open(origin, current, Direction.North);
        Open(origin, current, Direction.South);
        Open(origin, current, Direction.East);
        Open(origin, current, Direction.West);
    }

    public List<int> Shortest(RoomId origin, RoomId destination) => null;
}