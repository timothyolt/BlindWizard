using System;
using System.Collections.Generic;
using System.Linq;

namespace BlindWizard.Data
{
    public class Pathfinder
    {
        /// <summary>
        /// Affects how the algorithm values it's proximity to the destination
        /// </summary>
        private const int HeuristicWeight = 1;
        /// <summary>
        /// Affects how the algorithm values it's travel time from the origin
        /// </summary>
        private const int ActualWeight = 1;

        /// <summary>
        /// Link to the level for dimensional and room information
        /// </summary>
        private readonly WizardLevel _level;

        /// <summary>
        /// Proxy for WizardLevel.Level
        /// </summary>
        public int Level => _level.Level;
        /// <summary>
        /// Proxy for WizardLevel.Width;
        /// </summary>
        public int Width => _level.Width;
        /// <summary>
        /// Proxy for WizardLevel.Rooms
        /// </summary>
        private Room[,] Rooms => _level.Rooms;

        /// <summary>
        /// Creates a Pathfinder and links it to a given WizardLevl
        /// </summary>
        /// <param name="level">WizardLevel link</param>
        public Pathfinder(WizardLevel level)
        {
            _level = level;
            _origin = new Origin[Width, Width];
        }

        /// <summary>
        /// Optimstic heuristic using simple unobstructed move count for value
        /// </summary>
        private static int Heuristic(RoomId current, RoomId destination)
            => (Math.Abs(current.X - destination.X) + Math.Abs(current.Z - destination.Z)) * HeuristicWeight;

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
        /// Modularize the algorithm by origin room to reduce average memory footprint and improve code locality
        /// </summary>
        private readonly Origin[,] _origin;

        /// <summary>
        /// Finds the shortest acessible path from the origin room to the destination room
        /// </summary>
        /// <param name="origin">RoomId where the actor should begin</param>
        /// <param name="destination">RoomId where the actor desires to move to</param>
        /// <returns>A list of RoomId's from the destination to the origin,
        /// or a list containing only the destination if no such path can be found</returns>
        public List<RoomId> Shortest(RoomId origin, RoomId destination)
            => (_origin[origin.X, origin.Z] ??
                (_origin[origin.X, origin.Z] =
                    new Origin(this, origin))).Shortest(
                destination);

        /// <summary>
        /// An A* module, organized by origin RoomId
        /// </summary>
        private class Origin
        {

            /// <summary>
            /// Link to the parent Pathfinder
            /// </summary>
            private readonly Pathfinder _pathfinder;

            /// <summary>
            /// Proxy for Pathfinder.Width
            /// </summary>
            private int Width => _pathfinder.Width;
            /// <summary>
            /// Proxy for Pathfinder.Rooms
            /// </summary>
            private Room[,] Rooms => _pathfinder.Rooms;

            /// <summary>
            /// Creates a module of the Pathfinder object for a specific origin RoomId
            /// Can more or less be equated to a grouping of A* instances
            /// </summary>
            /// <param name="pathfinder">Parent pathfinder</param>
            /// <param name="origin"></param>
            public Origin(Pathfinder pathfinder, RoomId origin)
            {
                _pathfinder = pathfinder;
                _actual = new int[Width, Width];
                _state = new State[Width, Width];
                _open = new List<RoomId>();
                _parent = new RoomId?[Width, Width];
                _path = new List<RoomId>[Width, Width];
                // Open origin room
                _state[origin.X, origin.Z] = State.Open;
                _open.Add(origin);
            }

            /// <summary>
            /// Width by Width square grid, contains move cost to reach a room from an origin room
            /// </summary>
            private readonly int[,] _actual;

            /// <summary>
            /// Room state
            /// </summary>
            private readonly State[,] _state;

            /// <summary>
            /// A list of currently open rooms for faster iteration
            /// </summary>
            private readonly List<RoomId> _open;

            /// <summary>
            /// A grid of parenting id's tracing the current room towards the origin
            /// A room will be null if it is untouched
            /// </summary>
            private readonly RoomId?[,] _parent;

            /// <summary>
            /// A cache for previously found paths
            /// </summary>
            private readonly List<RoomId>[,] _path;

            /// <summary>
            /// The room is marked as open
            /// As long as the destination indicated by taking the given direction from the previous room is accessible.
            /// If already open and a better path is found, updates the actual cost and parent
            /// </summary>
            /// <param name="previous">The room to take the given direction from</param>
            /// <param name="direction">Determines the destination room from the previous room</param>
            private void Open(RoomId previous, Direction direction)
            {
                var current = previous[direction];
                if (!current.Bounds(_pathfinder.Width) ||                               // Out of bounds
                    Rooms[previous.X, previous.Z].Walls[direction].Gen) return;         // Through a wall
                if (_state[current.X, current.Z] == State.Untouched)                    // Not visited before
                {
                    _state[ current.X, current.Z] = State.Open;
                    _open.Add(current);
                    _actual[current.X, current.Z] =
                        _actual[previous.X, previous.Z] + ActualWeight;
                    _parent[current.X, current.Z] = previous;
                }
                else if (_state[current.X, current.Z] == State.Open &&  // Previously visited but still open
                         _actual[current.X, current.Z] >                // Previous visit was worse
                         _actual[previous.X, previous.Z] + ActualWeight)
                {
                    // Update actual cost and parent with new path
                    _actual[current.X, current.Z] =
                        _actual[previous.X, previous.Z] + ActualWeight;
                    _parent[current.X, current.Z] = previous;
                }
            }

            /// <summary>
            /// Marks a room as closed and attempts to open any acessible rooms
            /// </summary>
            /// <param name="current">The room to close</param>
            private void Close(RoomId current)
            {
                _state[current.X, current.Z] = State.Closed;
                _open.Remove(current);
                for (var d = (Direction) 0; d < (Direction) DirectionUtils.Count; d++)
                    Open(current, d);
            }


            /// <summary>
            /// Finds the shortest acessible path for this origin to the destination room
            /// </summary>
            /// <param name="destination">RoomId where the actor desires to move to</param>
            /// <returns>A list of RoomId's from the destination to the origin,
            /// or a list containing only the destination if no such path can be found</returns>
            public List<RoomId> Shortest(RoomId destination)
            {
                // Check cache
                if (_path[destination.X, destination.Z] != null)
                    return _path[ destination.X, destination.Z];
                // Proceed with A* by finding the lowest cost open room and closing it, functions take care of the rest
                while (_state[destination.X, destination.Z] != State.Closed && _open.Count > 0)
                {
                    var minRoomId = _open.First();
                    var minCost = _actual[minRoomId.X, minRoomId.Z] + Heuristic(minRoomId, destination);
                    foreach (var open in _open)
                    {
                        // Skip pits that are not the final destination
                        if (!Rooms[open.X, open.Z].FloorGen && open != destination)
                            continue;
                        var cost = _actual[open.X, open.Z] + Heuristic(open, destination);
                        if (cost > minCost)
                            continue;
                        minRoomId = open;
                        minCost = cost;
                    }
                    Close(minRoomId);
                }
                // Build path list by parenting
                var path = new List<RoomId> { destination };
                var last = destination;
                for (var i = 0; i < _actual[destination.X, destination.Z]; i++)
                {
                    var roomId = _parent[last.X, last.Z];
                    if (roomId == null)
                        break;
                    last = (RoomId) roomId;
                    path.Add(last);
                }
                _path[destination.X, destination.Z] = path;
                // Re-open destination if pit
                if (Rooms[destination.X, destination.Z].FloorGen) return path;
                _state[destination.X, destination.Z] = State.Open;
                _open.Add(destination);
                return path;
            }
        }
    }
}
