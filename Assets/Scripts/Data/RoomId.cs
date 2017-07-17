using System;

namespace BlindWizard.Data
{
    public struct RoomId
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
        public RoomId East => new RoomId(X + 1, Z);
        public RoomId West => new RoomId(X - 1, Z);

        public RoomId this[Direction direction]
        {
            get
            {
                switch (direction)
                {
                    case Direction.North:
                        return North;
                    case Direction.South:
                        return South;
                    case Direction.East:
                        return East;
                    case Direction.West:
                        return West;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
                }
            }
        }

        public Direction Orient(RoomId to)
        {
            if (to.X > X)
                return Direction.East;
            if (to.X < X)
                return Direction.West;
            if (to.Z > Z)
                return Direction.North;
            else
                return Direction.South;
        }

        /// <summary>
        /// Calculates distances between rooms, unobstructed, in turns
        /// Forms a diamond-like search perimeter
        /// </summary>
        /// <returns>Number of turns</returns>
        public int DistanceTurns(RoomId room) => Math.Abs(X - room.X) + Math.Abs(Z - room.Z);

        /// <summary>
        /// Calculated distance between rooms (squared), unobstructed, in euclidian distance
        /// Forms a circle-like search perimiter
        /// </summary>
        /// <param name="room">Number of rooms squared</param>
        /// <returns></returns>
        public int DistanceSquared(RoomId room) => (X - room.X) * (X - room.X) + (Z - room.Z) * (Z - room.Z);

        /// <summary>
        /// Calculated distance between rooms (linear), unobstructed, in euclidian distance
        /// Forms a circle-like search perimiter
        /// </summary>
        /// <param name="room">Fractional number of rooms</param>
        /// <returns></returns>
        public double Distance(RoomId room) => Math.Sqrt(DistanceSquared(room));

        public bool Bounds(int width) => X >= 0 && X < width && Z >= 0 && Z < width;

        public override string ToString() => $"X:{X}, Z:{Z}";

        public bool Equals(RoomId other) => X == other.X && Z == other.Z;

        public override bool Equals(object second)
            => !ReferenceEquals(null, second) && second is RoomId && Equals((RoomId) second);

        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Z;
            }
        }

        public static bool operator ==(RoomId first, RoomId second) => first.Equals(second);

        public static bool operator!=(RoomId first, RoomId second) => !first.Equals(second);
    }
}