using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class WizardLevel
{
    private static readonly Vector3 WallNsOffset = new Vector3(0, 0.5f, -0.5f);
    private static readonly Quaternion WallNsRotation = Quaternion.AngleAxis(90, Vector3.up);
    private static readonly Vector3 WallEwOffset = new Vector3(-0.5f, 0.5f, 0);
    private static readonly Quaternion WallEwRotation = Quaternion.identity;

    private readonly float _wOffset;
    private readonly int _y;
    private int _x;
    private int _z;

    public int Level { get; }
    public int Width { get; }
    public int Area { get; }
    public Room[,] Rooms;
    public GameObject Container { get; set; }

    public WizardLevel(int level)
    {
        Level = level;
        Width = level * 2 + 2;
        Area = Width * Width;
        _wOffset = Width / 2f - 0.5f;
        _y = Width * 2 + 1;
        Rooms = new Room[Width, Width];
        GenerateRooms();
        GeneratePits(Area / 16, Area / 32d, 0.25);
        GenerateWalls();
        GenerateMaze();
        GenerateShimmers(Width / 2, Width / 4d, 0.5f, 1);
        // GenerateEnemies(Width / 2, Width / 4d, 0.5f, 1, enemyPrefab);
    }

    private static double ComplexProbabillity(double at, double desired, double spread = 1)
        => Math.Pow(Math.Pow(10, Math.Log10(desired) / (at * spread)), spread);

    private static void WhereCount(int width, Func<int, int, bool> inner, Func<int, bool> outer,
        Action<int, int> action)
    {
        var count = 1;
        do
        {
            int x;
            int z;
            do
            {
                x = Random.Range(0, width);
                z = Random.Range(0, width);
            } while (inner(x, z));
            action(x, z);
        } while (outer(count++));
    }

    private void GenerateRooms()
    {
        Debug.Log(nameof(GenerateRooms));
        for (var x = 0; x < Width; x++)
        for (var z = 0; z < Width; z++)
            Rooms[x, z] = new Room {FloorGen = true};
    }

    private void GeneratePits(int max, double at, double desired, double spread = 1)
    {
        Debug.Log(nameof(GeneratePits));
        var probability = ComplexProbabillity(at, desired, spread);
        WhereCount(Width,
            (x, z) => !Rooms[x, z].FloorGen,
            count => count < max && Random.Range(0, 1f) < probability,
            (x, z) => Rooms[x, z].FloorGen = false);
    }

    private void GenerateWalls()
    {
        Debug.Log(nameof(GenerateWalls));
        for (var x = 0; x < Width + 1; x++)
        for (var z = 0; z < Width + 1; z++)
        {
            if (x < Width && z < Width)
            {
                Rooms[x, z].WallNorth = new Wall {Gen = true};
                Rooms[x, z].WallEast = new Wall {Gen = true};
                if (z > 0 && x < Width)
                    Rooms[x, z - 1].WallSouth = Rooms[x, z].WallNorth;
                if (x > 0 && z < Width)
                    Rooms[x - 1, z].WallWest = Rooms[x, z].WallEast;
            }
            else
            {
                if (x > 0 && z < Width)
                    Rooms[x - 1, z].WallWest = new Wall {Gen = true};
                if (z > 0 && x < Width)
                    Rooms[x, z - 1].WallSouth = new Wall {Gen = true};
            }
        }
    }

    private void GenerateMaze()
    {
        Debug.Log(nameof(GenerateMaze));
        Room Room(Vector2 id) => Rooms[(int) id.x, (int) id.y];
        // TODO(timothyolt): make visited and border 2D boolean arrays
        var path = new Stack<Vector2>();
        var visited = new List<Vector2>();
        var border = new List<Vector2>();
        for (var i = 0; i < Width; i++)
        {
            border.Add(new Vector2(i, -1));
            border.Add(new Vector2(i, Width));
        }
        for (var i = 0; i < Width; i++)
        {
            border.Add(new Vector2(-1, i));
            border.Add(new Vector2(Width, i));
        }

        // Find a non-pit starting point
        while (true)
        {
            Debug.Log(Width);
            var roomId = new Vector2(Random.Range(0, Width), Random.Range(0, Width));
            if (!Room(roomId).FloorGen)
                continue;
            path.Push(roomId);
            visited.Add(roomId);
            break;
        }
        // pick random n/s/e/w
        // check to see if selection is in the path array
        // if so, add it to Path, cut out the wall, set tileTested to new selection
        // else, try again with new random direction
        while (path.Count > 0)
        {
            var roomId = path.Peek();
            // pit detection
            if (!Room(roomId).FloorGen)
            {
                if (roomId.y < Width - 1)
                    Room(roomId).WallSouth.Gen = false;
                if (roomId.y > 0)
                    Room(roomId).WallNorth.Gen = false;
                if (roomId.x < Width - 1)
                    Room(roomId).WallEast.Gen = false;
                if (roomId.x > 0)
                    Room(roomId).WallWest.Gen = false;
                path.Pop();
                continue;
            }
            // dead end detection
            if (visited.Concat(border).Intersect(new[]
                    {
                        roomId + Vector2.up,
                        roomId + Vector2.down,
                        roomId + Vector2.right,
                        roomId + Vector2.left
                    })
                    .Count() == 4)
            {
                path.Pop();
                continue;
            }
            // select random direction
            Vector2 nsew;
            switch (Random.Range(0, 4))
            {
                case 0:
                    nsew = Vector2.up;
                    break;
                case 1:
                    nsew = Vector2.down;
                    break;
                case 2:
                    nsew = Vector2.right;
                    break;
                case 3:
                    nsew = Vector2.left;
                    break;
                default:
                    throw new IndexOutOfRangeException("Only 4 cardinal directions allowed");
            }
            roomId += nsew;
            // check for used rooms
            if (visited.Concat(border).Contains(roomId)) continue;
            // mark next room
            path.Push(roomId);
            visited.Add(roomId);
            // break wall
            if (nsew == Vector2.up)
                Room(roomId).WallNorth.Gen = false;
            else if (nsew == Vector2.down)
                Room(roomId).WallSouth.Gen = false;
            else if (nsew == Vector2.right)
                Room(roomId).WallEast.Gen = false;
            else if (nsew == Vector2.left)
                Room(roomId).WallWest.Gen = false;
            else throw new IndexOutOfRangeException("Only 4 cardinal directions allowed");
        }
    }

    private void GenerateShimmers(int max, double at, double desired, double spread)
    {
        Debug.Log(nameof(GenerateShimmers));
        var probability = ComplexProbabillity(at, desired, spread);
        WhereCount(Width,
            (x, z) => !Rooms[x, z].FloorGen || Rooms[x, z].EnemyGen || Rooms[x, z].ShimmerGen,
            count => count < max && Random.Range(0, 1f) < probability,
            (x, z) => Rooms[x, z].ShimmerGen = true);
    }

    private void GenerateEnemies(int max, double at, double desired, double spread)
    {
        Debug.Log(nameof(GenerateEnemies));
        var probability = ComplexProbabillity(at, desired, spread);
        WhereCount(Width,
            (x, z) => !Rooms[x, z].FloorGen || Rooms[x, z].EnemyGen || Rooms[x, z].ShimmerGen,
            count => count < max && Random.Range(0, 1f) < probability,
            (x, z) => Rooms[x, z].EnemyGen = true);
    }

    public void Instantiate(GameObject floorPrefab, GameObject shimmerPrefab, GameObject enemyPrefab, GameObject wallPrefab)
    {
        Debug.Log(nameof(Instantiate));
        Container = new GameObject($"Level Container {Width / 2 - 1}");
        for (var x = 0; x < Width + 1; x++)
        for (var z = 0; z < Width + 1; z++)
        {
            void InstantiateWall(Wall wall, GameObject container, Vector3 offset, Quaternion rotation)
            {
                if (wall.Gen && wall.GameObject == null)
                    wall.GameObject = Object.Instantiate(wallPrefab,
                        wallPrefab.transform.position + container.transform.position + offset,
                        wallPrefab.transform.rotation * rotation, container.transform);
            }
            if (x < Width && z < Width)
            {
                var room = Rooms[x, z];
                room.Container = new GameObject($"Room Container {x}, {z}");
                room.Container.transform.SetParent(Container.transform);
                room.Container.transform.position = new Vector3(x - _wOffset, -_y, z - _wOffset);
                if (room.FloorGen)
                    room.Floor = Object.Instantiate(floorPrefab, room.Container.transform);
                if (room.ShimmerGen)
                    room.Shimmer =
                        Object.Instantiate(shimmerPrefab, room.Container.transform.position + Vector3.up,
                            shimmerPrefab.transform.rotation, room.Container.transform);
                if (room.EnemyGen)
                    room.Enemy =
                        Object.Instantiate(enemyPrefab, room.Container.transform.position + Vector3.up,
                            shimmerPrefab.transform.rotation, room.Container.transform);
                InstantiateWall(room.WallNorth, room.Container, WallNsOffset, WallNsRotation);
                InstantiateWall(room.WallEast, room.Container, WallEwOffset, WallEwRotation);
            }
            else
            {
                if (z > 0 && x < Width)
                    InstantiateWall(Rooms[x, z - 1].WallSouth, Rooms[x, z - 1].Container, WallNsOffset + Vector3.forward, WallNsRotation);
                if (x > 0 && z < Width)
                    InstantiateWall(Rooms[x - 1, z].WallWest, Rooms[x - 1, z].Container, WallEwOffset + Vector3.right, WallEwRotation);
            }
        }
    }

    public void Destroy()
    {
        // TODO (timothyolt): actually do something
    }
}
