using System;
using System.Collections.Generic;
using System.Threading;
using Data;
using Random = System.Random;

namespace MonoBehaviours
{
    public class Maze
    {
        private struct RoomId
        {
            public RoomId(int x, int z)
            {
                X = x;
                Z = z;
            }

            public int X { get; }
            public int Z { get; }

            public RoomId North => new RoomId(X, Z + 1);
            public RoomId South => new RoomId(X, Z - 1);
            public RoomId East  => new RoomId(X + 1, Z);
            public RoomId West  => new RoomId(X - 1, Z);
        }

        private static int _seed = Environment.TickCount;

        private readonly Random _random;

        private readonly Room[,] _rooms;
        private readonly int _width;
        // Current path from the starting point
        private readonly Stack<RoomId> _path;
        // 2D array of locations, and whether they are visited (shouldn't be considered for exploration).
        // 0,0 on the maze is 1,1 in this array.
        private readonly bool[,] _visited;

        public Maze(Room[,] rooms)
        {
            //Debug.Log("Creating maze");
            _random = new Random(Interlocked.Increment(ref _seed));
            _rooms = rooms;
            _width = rooms.GetLength(0);
            _path = new Stack<RoomId>();
            _visited = new bool[_width + 2, _width + 2];
            VisitBorder();
            FindValidStart();
            Generate();
        }

        /// <summary>
        /// Easy get a room by id
        /// </summary>
        /// <param name="id">Id of room to get</param>
        /// <returns></returns>
        private Room Room(RoomId id) => _rooms[id.X, id.Z];

        /// <summary>
        /// Mark a location as visited
        /// </summary>
        /// <param name="id">Id to mark</param>
        private void Visit(RoomId id)
        {
            _path.Push(id);
            _visited[id.X + 1, id.Z + 1] = true;
        }

        /// <summary>
        /// Whether the Maze is currently being calculated
        /// </summary>
        private bool InProgress => _path.Count > 0;

        /// <summary>
        /// Returns current room id
        /// </summary>
        private RoomId Current => _path.Peek();

        /// <summary>
        /// Backtraces the room id
        /// </summary>
        private void Backtrace() => _path.Pop();

        /// <summary>
        /// Index range -1 to Width
        /// </summary>
        /// <param name="id">Id to check</param>
        /// <returns></returns>
        private bool IsVisited(RoomId id) => _visited[id.X + 1, id.Z + 1];

        /// <summary>
        /// Returns true if a location has been
        /// </summary>
        /// <param name="id">Id to validate</param>
        /// <returns>Whether the given room is a valid movement</returns>
        private bool Validate(RoomId id)
        {
            // check for used rooms
            if (IsVisited(id)) return false;
            // mark next room
            Visit(id);
            return true;
        }

        /// <summary>
        /// Set border to 'visited' to prevent overrun
        /// </summary>
        private void VisitBorder()
        {
            for (var i = 1; i < _width + 1; i++)
            {
                _visited[i, 0] = true;
                _visited[0, i] = true;
                _visited[i, _width + 1] = true;
                _visited[_width + 1, i] = true;
            }
        }

        /// <summary>
        /// Find a non-pit starting point
        /// </summary>
        private void FindValidStart()
        {
            while (true)
            {
                var roomId = new RoomId (_random.Next(0, 4), _random.Next(0, 4));
                if (!Room(roomId).FloorGen)
                    continue;
                Visit(roomId);
                break;
            }
        }

        /// <summary>
        /// Iterative backtracing modified algorithm
        /// </summary>
        /// <exception cref="IndexOutOfRangeException">Should never happen</exception>
        private void Generate()
        {
            while (InProgress)
            {
                //Debug.Log("Creating maze");
                var roomId = Current;
                // pit detection
                if (!Room(roomId).FloorGen)
                {
                    if (roomId.Z > 0)
                        Room(roomId).WallNorth.Gen = false;
                    if (roomId.Z < _width - 1)
                        Room(roomId).WallSouth.Gen = false;
                    if (roomId.X > 0)
                        Room(roomId).WallEast.Gen = false;
                    if (roomId.X < _width - 1)
                        Room(roomId).WallWest.Gen = false;
                    Backtrace();
                    continue;
                }
                // dead end detection
                if (IsVisited(roomId.North) &&
                    IsVisited(roomId.South) &&
                    IsVisited(roomId.East) &&
                    IsVisited(roomId.West))
                {
                    Backtrace();
                    continue;
                }
                // select random direction
                switch (_random.Next(0, 4))
                {
                    case 0:
                        roomId = roomId.North;
                        if (!Validate(roomId)) continue;
                        Room(roomId).WallNorth.Gen = false;
                        break;
                    case 1:
                        roomId = roomId.South;
                        if (!Validate(roomId)) continue;
                        Room(roomId).WallSouth.Gen = false;
                        break;
                    case 2:
                        roomId = roomId.East;
                        if (!Validate(roomId)) continue;
                        Room(roomId).WallEast.Gen = false;
                        break;
                    case 3:
                        roomId = roomId.West;
                        if (!Validate(roomId)) continue;
                        Room(roomId).WallWest.Gen = false;
                        break;
                    default:
                        throw new IndexOutOfRangeException("Only 4 cardinal directions allowed");
                }
            }
        }
    }
}
