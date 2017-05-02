using System;
using System.Collections.Generic;
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
        var maze = new Maze(Rooms);
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
