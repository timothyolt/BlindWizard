using System;

namespace Blindwizard.Data
{
    public enum Direction
    {
        North,
        South,
        East,
        West
    }

    public static class DirectionUtils
    {
        public const int Count = 4;

        public static int Rotation(this Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                    return 0;
                case Direction.South:
                    return 180;
                case Direction.East:
                    return 90;
                case Direction.West:
                    return 270;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }
    }
}