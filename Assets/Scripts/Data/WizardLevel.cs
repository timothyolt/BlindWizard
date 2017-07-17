using System;
using System.Collections;
using System.Threading;
using BlindWizard.MonoBehaviours;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace BlindWizard.Data
{
    public class WizardLevel
    {
        public const float RoomScale = 2f;
        public const float FloorHeight = 2f;
        public const float WallHeight = 4f;
        public const float WallVerticalOffset = (WallHeight - FloorHeight) / 2;
        private static readonly Vector3 WallNorthOffset = new Vector3(0, WallVerticalOffset, RoomScale / 2);
        private static readonly Vector3 WallSouthOffset = new Vector3(0, WallVerticalOffset, -RoomScale / 2);
        private static readonly Quaternion WallNsRotation = Quaternion.AngleAxis(90, Vector3.up);
        private static readonly Vector3 WallEastOffset = new Vector3(RoomScale / 2, WallVerticalOffset, 0);
        private static readonly Vector3 WallWestOffset = new Vector3(-RoomScale / 2, WallVerticalOffset, 0);
        private static readonly Quaternion WallEwRotation = Quaternion.identity;

        private readonly float _wOffset;
        private readonly int _y;
        private int _x;
        private int _z;

        public int Level { get; }
        public int Width { get; }
        public int Area { get; }

        //TODO: using Rooms while the Maze is being generated is likely not threadsafe
        // Experiment with making them structs for further immutability
        public readonly Room[,] Rooms;
        public Room this[int x, int z] => Rooms[x, z];
        public Room this[RoomId roomId] => Rooms[roomId.X, roomId.Z];
        private Maze _maze;
        private Thread _mazeTask;
        public GameObject Container { get; set; }
        public Pathfinder Pathfinder { get; }

        public WizardLevel(int level)
        {
            Level = level;
            Width = (level + 1) * 2 + 2;
            Area = Width * Width;
            _wOffset = Width * RoomScale / 2f;
            _y = Width * 2 + 1;
            Rooms = new Room[Width, Width];
            Pathfinder = new Pathfinder(this);
        }

        protected static double ComplexProbabillity(double at, double desired, double spread = 1)
            => Math.Pow(Math.Pow(10, Math.Log10(desired) / (at * spread)), spread);

        protected static void WhereCount(int width, Func<int, int, bool> inner, Func<int, bool> outer,
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

        protected virtual void GenerateRooms()
        {
            //Debug.Log(Level + nameof(GenerateRooms));
            for (var x = 0; x < Width; x++)
            for (var z = 0; z < Width; z++)
                Rooms[x, z] = new Room {FloorGen = true};
        }

        protected virtual void GeneratePits(int max, double at, double desired, double spread = 1)
        {
            //Debug.Log(Level + nameof(GeneratePits));
            var probability = ComplexProbabillity(at, desired, spread);
            WhereCount(Width,
                (x, z) => !Rooms[x, z].FloorGen,
                count => count < max && Random.Range(0, 1f) < probability,
                (x, z) => Rooms[x, z].FloorGen = false);
        }

        protected virtual void GenerateWalls()
        {
            //Debug.Log(Level + nameof(GenerateWalls));
            for (var x = 0; x < Width + 1; x++)
            for (var z = 0; z < Width + 1; z++)
            {
                if (x < Width && z < Width)
                {
                    Rooms[x, z].Walls[Direction.South] = new Wall {Gen = true};
                    Rooms[x, z].Walls[Direction.West] = new Wall {Gen = true};
                    if (z > 0 && x < Width)
                        Rooms[x, z - 1].Walls[Direction.North] = Rooms[x, z].Walls[Direction.South];
                    if (x > 0 && z < Width)
                        Rooms[x - 1, z].Walls[Direction.East] = Rooms[x, z].Walls[Direction.West];
                }
                else
                {
                    if (x > 0 && z < Width)
                        Rooms[x - 1, z].Walls[Direction.East] = new Wall {Gen = true};
                    if (z > 0 && x < Width)
                        Rooms[x, z - 1].Walls[Direction.North] = new Wall {Gen = true};
                }
            }
        }

        protected virtual void GenerateShimmers(int max, double at, double desired, double spread)
        {
            //Debug.Log(Level + nameof(GenerateShimmers));
            var probability = ComplexProbabillity(at, desired, spread);
            WhereCount(Width,
                (x, z) => !Rooms[x, z].FloorGen || Rooms[x, z].EnemyGen || Rooms[x, z].ShimmerGen,
                count => count < max && Random.Range(0, 1f) < probability,
                (x, z) => Rooms[x, z].ShimmerGen = true);
        }

        protected virtual void GenerateEnemies(int max, double at, double desired, double spread)
        {
            //Debug.Log(Level + nameof(GenerateEnemies));
            var probability = ComplexProbabillity(at, desired, spread);
            WhereCount(Width,
                (x, z) => !Rooms[x, z].FloorGen || Rooms[x, z].EnemyGen || Rooms[x, z].ShimmerGen,
                count => count < max && Random.Range(0, 1f) < probability,
                (x, z) => Rooms[x, z].EnemyGen = true);
        }

        protected virtual void GenerateMaze()
        {
            //Debug.Log(Level + nameof(GenerateMaze));
            _mazeTask = new Thread(() =>
            {
                //Debug.Log("Maze creating");
                _maze = new Maze(Rooms);
                //Debug.Log("Maze finished");
            }) {Name = $"WizardLevel-{Level}", IsBackground = true};
            _mazeTask.Start();
            //Debug.Log(Level + nameof(GenerateMaze) + " Started");
        }

        public virtual void Generate() {
            GenerateRooms();
            GeneratePits(Area / 16, Area / 32d, 0.25);
            GenerateWalls();
            GenerateShimmers(Width / 2, Width / 4d, 0.5f, 1);
            GenerateEnemies(Width / 2, Width / 4d, 0.5f, 1);
            GenerateMaze();
        }

        public virtual bool IsDone => _maze != null;

#if DEVELOPMENT_BUILD || UNITY_EDITOR
        public virtual IEnumerator Instantiate(GameObject floorPrefab, GameObject pitPrefab, GameObject shimmerPrefab,
            GameObject enemyPrefab, GameObject wallPrefab, GameObject pathPrefab, DrawableSurfaceSet set, GameObject devTextPrefab)
#else
        public IEnumerator Instantiate(GameObject floorPrefab, GameObject pitPrefab, GameObject shimmerPrefab,
            GameObject enemyPrefab, GameObject wallPrefab, DrawableSurfaceSet set, GameObject pathPrefab)
#endif
        {
            //Debug.Log(Level + nameof(Instantiate));
            Container = new GameObject($"Level Container {Width / 2 - 1}");
            for (var x = 0; x < Width + 1; x++)
            for (var z = 0; z < Width + 1; z++)
            {
                void InstantiateWall(Wall wall, GameObject container, Vector3 offset, Quaternion rotation, string name)
                {
                    if (wall.Gen && wall.GameObject == null)
                    {
                        (wall.GameObject = Object.Instantiate(wallPrefab,
                            wallPrefab.transform.position + container.transform.position + offset,
                            wallPrefab.transform.rotation * rotation, container.transform)).name = name;
                        wall.GameObject.GetComponent<DrawableSurface>().Set = set;
                    }
                }
                if (x < Width && z < Width)
                {
                    var room = Rooms[x, z];
                    room.Container = new GameObject($"Room Container {x}, {z}")
                    {
                        transform = {position = new Vector3(x * RoomScale - _wOffset, -_y, z * RoomScale - _wOffset)}
                    };
                    room.Container.transform.SetParent(Container.transform);
                    room.Path = Object.Instantiate(pathPrefab, room.Container.transform);
                    room.Path.name = $"Path {x}, {z}";
#if DEVELOPMENT_BUILD || UNITY_EDITOR
                    room.DevText = Object.Instantiate(devTextPrefab, room.Container.transform);
#endif
                    if (room.FloorGen)
                    {
                        room.Floor = Object.Instantiate(floorPrefab, room.Container.transform);
                        room.Floor.name = $"Floor {x}, {z}";
                        var floor = room.Floor.GetComponent<Floor>();
                        floor.Level = Level;
                        floor.Position = new RoomId(x, z);
                    }
                    else
                    {
                        room.Pit = Object.Instantiate(pitPrefab, room.Container.transform);
                        room.Pit.name = $"Pit {x}, {z}";
                        var floor = room.Pit.GetComponent<Floor>();
                        floor.Level = Level;
                        floor.Position = new RoomId(x, z);
                    }
                    if (room.ShimmerGen)
                        (room.Shimmer =
                            Object.Instantiate(shimmerPrefab, room.Container.transform.position + Vector3.up,
                                shimmerPrefab.transform.rotation, room.Container.transform)).name = $"Shimmer {x}, {z}";
                    if (room.EnemyGen)
                    {
                        (room.Enemy =
                            Object.Instantiate(enemyPrefab)).name = $"Enemy [{Level}], {x}, {z}";
                        var enemyMovement = room.Enemy.GetComponent<EnemyMovement>();
                        if (enemyMovement != null)
                        {
                            enemyMovement.Level = Level;
                            enemyMovement.Position = new RoomId(x, z);
                        }
                    }
                    InstantiateWall(room.Walls[Direction.South], room.Container, WallSouthOffset, WallNsRotation,
                        $"Wall North {x}, {z}");
                    InstantiateWall(room.Walls[Direction.West], room.Container, WallWestOffset, WallEwRotation,
                        $"Wall East {x}, {z}");
                }
                else
                {
                    if (z > 0 && x < Width)
                        InstantiateWall(Rooms[x, z - 1].Walls[Direction.North], Rooms[x, z - 1].Container,
                            WallNorthOffset, WallNsRotation, $"Wall South {x}, {z - 1}");
                    if (x > 0 && z < Width)
                        InstantiateWall(Rooms[x - 1, z].Walls[Direction.East], Rooms[x - 1, z].Container,
                            WallEastOffset, WallEwRotation, $"Wall West {x - 1}, {z}");
                }
                yield return null;
            }
        }

        public void Destroy()
        {
            Object.Destroy(Container);
            // TODO (timothyolt): actually do something
        }
    }
}
