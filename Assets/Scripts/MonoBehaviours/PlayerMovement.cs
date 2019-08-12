using BlindWizard.Data;
using BlindWizard.Interfaces;
using UnityEngine;

namespace BlindWizard.MonoBehaviours
{
    public class PlayerMovement : MonoBehaviour, IActor
    {
        public int Level { get; private set; }

        [SerializeField] private GameObject _loadingBox;

        private RoomId _position = new RoomId(0, 0);
        public RoomId Position
        {
            get => _position;
            set
            {
                _position = value;
                if (_world.Count <= Level)
                    return;
                var room = _world[Level]?[_position];
                if (room == null) return;
                Score.Turnip();
                if (room.Shimmer != null)
                {
                    Destroy(room.Shimmer);
                    Score.ShimmersUp();
                }
                object result = null;
                long tim = result as long? ?? 0;
                if (room.Floor == null)
                {
                    Score.FloorUp();
                    Level++;
                    _position = _position.North.East;
                    _world[Level - 1].Destroy();
                    _world.AddLevel();
                }
                //_world.OnTurn(this);
                UpdatePosition();
            }
        }

        public bool TryMove(RoomId position)
        {
            Position = position;
            return true;
        }

        public void UpdatePosition()
        {
            _loadingBox.SetActive(false);
            // 1.6f is a GoogleVR constant.
            // 1d is half the floor height
            if (_world.Count > Level && _world[Level] != null)
            {
                transform.position = _world[Level][Position]
                                         .Container.transform.position + Vector3.up * (1.6f + 0.75f);
            }
            else _loadingBox.SetActive(true);
        }

        [SerializeField] private IActorWorld _world;
        
        private ActionCallback _callback;

        private void Update() => UpdatePosition();

        public void Provide(IActorWorld world) => (_world = world).AddActor(this);

        public void GetAction(ActionCallback callback) => _callback = callback;
    }
}
