using System;
using System.Collections.Generic;
using System.Threading;
using Random = System.Random;

public class Maze
{

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
            var roomId = new RoomId (_random.Next(0, DirectionUtils.Count), _random.Next(0, DirectionUtils.Count));
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
                for (var d = (Direction) 0; d < (Direction) DirectionUtils.Count; d++)
                    if (roomId[d].Bounds(_width))
                        Room(roomId).Walls[d].Gen = false;
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
            var direction = (Direction) _random.Next(0, DirectionUtils.Count);
            roomId = roomId[direction];
            if (!Validate(roomId)) continue;
            Room(roomId).Walls[direction].Gen = false;
        }
    }
}
