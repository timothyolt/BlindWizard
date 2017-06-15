using BlindWizard.Data;
using UnityEngine;

namespace BlindWizard.MonoBehaviours
{
    public class PlayerMovement : MonoBehaviour
    {
        public int Level { get; private set; }

        [SerializeField] private GameObject _loadingBox;

        private RoomId _position;
        public RoomId Position
        {
            get => _position;
            set
            {
                _position = value;
                if (_world.Levels.Count <= Level)
                    return;
                var room = _world.Levels[Level]?.Rooms[_position.X, _position.Z];
                if (room == null) return;
                Score.Turnip();
                if (room.Shimmer != null)
                {
                    Destroy(room.Shimmer);
                    Score.ShimmersUp();
                }
                if (room.Floor == null)
                {
                    Score.FloorUp();
                    Level++;
                    _position = _position.North.East;
                    _world.Levels[Level - 1].Destroy();
                    _world.AddLevel();
                }
                UpdatePosition();
            }
        }

        public void UpdatePosition()
        {
            _loadingBox.SetActive(false);
            // 1.6f is a GoogleVR constant.
            if (_world.Levels.Count > Level && _world.Levels[Level] != null)
            {
                transform.position = _world.Levels[Level]
                                         .Rooms[_position.X, _position.Z]
                                         .Container.transform.position + Vector3.up * 1.6f;
            }
            else _loadingBox.SetActive(true);
        }

        [SerializeField] private World _world;

        private void Start()
        {
            Level = 0;
            Position = new RoomId(0, 0);
        }

        private void Update()
        {
            UpdatePosition();
        }
    }
}
