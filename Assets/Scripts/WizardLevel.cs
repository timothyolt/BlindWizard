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

    public int Level { get; }
    public int Width { get; }
    public int Area { get; }
    public Room[,] Rooms;

    public WizardLevel(int level, GameObject floorPrefab,
        GameObject wallPrefab, GameObject shimmerPrefab, GameObject enemyPrefab)
    {
        Level = level;
        Width = level * 2 + 2;
        Area = Width * Width;
        Rooms = new Room[Width, Width];
        CreateContainers();
        CreateFloor(floorPrefab);
        CreateWalls(wallPrefab);
        Pitify(Area / 16, Area / 32d, 0.25);
        Wallify();
        PopulateShimmers(Width / 2, Width / 4d, 0.5f, 1, shimmerPrefab);
        // PopulateEnemies(Width / 2, Width / 4d, 0.5f, 1, enemyPrefab);
    }

    private static double ComplexProbabillity(double at, double desired, double spread = 1)
        => Math.Pow(Math.Pow(10, Math.Log10(desired) / (at * spread)), spread);

    private static void FloorEach(int width, Func<int, int, bool> inner, Func<int, bool> outer,
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

    private void CreateContainers()
    {
        var wOffset = Width / 2f - 0.5f;
        for (var x = 0; x <Width; x++)
        for (var z = 0; z < Width; z++)
        {
            Rooms[x, z] = new Room {Container = new GameObject($"Room Container ({x},{Level},{z})")};
            Rooms[x, z].Container.transform.position = new Vector3(x - wOffset, -Width * 2 + 1, z - wOffset);
        }
    }

    private void CreateFloor(GameObject prefab)
    {
        for (var x = 0; x <Width; x++)
        for (var z = 0; z < Width; z++)
            Rooms[x, z].Floor = Object.Instantiate(prefab, Rooms[x, z].Container.transform);
    }

    private void CreateWalls(GameObject wallPrefab)
    {
        for (var x = 0; x < Width + 1; x++)
        for (var z = 0; z < Width + 1; z++)
        {
            if (x < Width && z < Width)
            {
                Rooms[x, z].WallNorth = Object.Instantiate(wallPrefab,
                    wallPrefab.transform.position + Rooms[x, z].Container.transform.position + WallNsOffset,
                    wallPrefab.transform.rotation * WallNsRotation, Rooms[x, z].Container.transform);
                Rooms[x, z].WallEast = Object.Instantiate(wallPrefab,
                    wallPrefab.transform.position + Rooms[x, z].Container.transform.position + WallEwOffset,
                    wallPrefab.transform.rotation * WallEwRotation, Rooms[x, z].Container.transform);
                if (z > 0 && x < Width)
                    Rooms[x, z - 1].WallSouth = Rooms[x, z].WallNorth;
                if (x > 0 && z < Width)
                    Rooms[x - 1, z].WallWest = Rooms[x, z].WallEast;
            }
            else
            {
                if (x > 0 && z < Width)
                    Rooms[x - 1, z].WallWest = Object.Instantiate(wallPrefab,
                        wallPrefab.transform.position + Rooms[x - 1, z].Container.transform.position + WallEwOffset + Vector3.right,
                        wallPrefab.transform.rotation * WallEwRotation, Rooms[x - 1, z].Container.transform);
                if (z > 0 && x < Width)
                    Rooms[x, z - 1].WallSouth = Object.Instantiate(wallPrefab,
                        wallPrefab.transform.position + Rooms[x, z - 1].Container.transform.position + WallNsOffset + Vector3.forward,
                        wallPrefab.transform.rotation * WallNsRotation, Rooms[x, z - 1].Container.transform);
            }
        }
    }

    private void Pitify(int max, double at, double desired, double spread = 1)
    {
        var probability = ComplexProbabillity(at, desired, spread);
        FloorEach(Width,
            (x, z) => Rooms[x, z].Floor == null,
            count => count < max && Random.Range(0, 1f) < probability,
            (x, z) =>
            {
                Object.Destroy(Rooms[x, z].Floor);
                Rooms[x, z].Floor = null;
            });
    }

    private void Wallify()
    {
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

        path.Push(new Vector2(Random.Range(0, Width),
            Random.Range(0, Width)));
        visited.Add(path.Peek());

        Room Room(Vector2 id) => Rooms[(int) id.x, (int) id.y];
        while (path.Count > 0)
        {
            var roomId = path.Peek();
            // pick random n/s/e/w
            // check to see if selection is in the path array
            // if so, add it to Path, cut out the wall, set tileTested to new selection
            // else, try again with new random direction

            // pit detection
            if (Room(roomId).Floor == null)
            {
                if (roomId.y < Width - 1)
                    Object.Destroy(Room(roomId).WallSouth);
                if (roomId.y > 0)
                    Object.Destroy(Room(roomId).WallNorth);
                if (roomId.x < Width - 1)
                    Object.Destroy(Room(roomId).WallWest);
                if (roomId.x > 0)
                    Object.Destroy(Room(roomId).WallEast);
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
                Object.Destroy(Room(roomId).WallNorth);
            else if (nsew == Vector2.down)
                Object.Destroy(Room(roomId).WallSouth);
            else if (nsew == Vector2.right)
                Object.Destroy(Room(roomId).WallEast);
            else if (nsew == Vector2.left)
                Object.Destroy(Room(roomId).WallWest);
            else throw new IndexOutOfRangeException("Only 4 cardinal directions allowed");
        }
    }

    private void PopulateShimmers(int max, double at, double desired, double spread, GameObject shimmerPrefab)
    {
        var probability = ComplexProbabillity(at, desired, spread);
        FloorEach(Width,
            (x, z) => Rooms[x, z].Floor == null || Rooms[x, z].Enemy != null,
            count => count < max && Random.Range(0, 1f) < probability,
            (x, z) =>
                Rooms[x, z].Shimmer =
                    Object.Instantiate(shimmerPrefab, Rooms[x, z].Container.transform.position + Vector3.up,
                        shimmerPrefab.transform.rotation, Rooms[x, z].Container.transform)
        );
    }

    private void PopulateEnemies(int max, double at, double desired, double spread, GameObject enemyPrefab)
    {
        var probability = ComplexProbabillity(at, desired, spread);
        FloorEach(Width,
            (x, z) => Rooms[x, z].Floor == null || Rooms[x, z].Enemy != null,
            count => count < max && Random.Range(0, 1f) < probability,
            (x, z) =>
                Rooms[x, z].Enemy =
                    Object.Instantiate(enemyPrefab, Rooms[x, z].Container.transform.position + Vector3.up,
                        enemyPrefab.transform.rotation, Rooms[x, z].Container.transform)
        );
    }
}
