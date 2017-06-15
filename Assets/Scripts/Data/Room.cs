using UnityEngine;

namespace BlindWizard.Data
{
    public class Room
    {
        public bool IsGenerated { get; set; }
        public bool FloorGen { get; set; }
        public bool EnemyGen { get; set; }
        public bool ShimmerGen { get; set; }
        /// <summary>
        /// GameObject that always exists within a room
        /// </summary>
        public GameObject Container { get; set; }
        public GameObject Path { get; set; }
        /// <summary>
        /// Floor GameObject of a room, will be null if this is a "pit room"
        /// </summary>
        public GameObject Floor { get; set; }

        public class WallCollection
        {
            private readonly Wall[] _walls = new Wall[DirectionUtils.Count];

            public Wall this[Direction direction]
            {
                get => _walls[(int) direction];
                set => _walls[(int) direction] = value;
            }
        }
        public WallCollection Walls { get; } = new WallCollection();

        /// <summary>
        /// GameObject for an enemy in the room. Will be null if there is no enemy in the room
        /// </summary>
        public GameObject Enemy { get; set; }
        /// <summary>
        /// GameObject for a Shimmer in the room. Will be null if there is no shimmer
        /// </summary>
        public GameObject Shimmer { get; set; }
        /// <summary>
        /// GameObject for a Pit, used for player movement
        /// </summary>
        public GameObject Pit { get; set; }
    }
}
