using System;

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

public bool Bounds(int width) => X >= 0 && X < width && Z >= 0 && Z < width;
}