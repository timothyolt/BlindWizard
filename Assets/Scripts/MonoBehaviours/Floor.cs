using System.Text;
using BlindWizard.Data;
using UnityEngine;

namespace BlindWizard.MonoBehaviours
{
    [RequireComponent(typeof(MeshRenderer))]
    public class Floor : MonoBehaviour
    {
        private World _world;
        private PlayerMovement _player;

        public RoomId Position { get; set; }
        public int Level { get; set; }

        private void Start ()
        {
            _world = FindObjectOfType<World>();
            _player = FindObjectOfType<PlayerMovement>();
        }

        public void SetGazedAt(bool gazed)
        {
            if (!gazed)
            {
                _world.Path = null;
                return;
            }
            var path = _world.Levels[Level].Pathfinder.Shortest(_player.Position, Position);
            _world.Path = path;
#if DEBUG
            var stringBuilder = new StringBuilder();
            foreach (var roomId in path)
                stringBuilder.Append(roomId);
            if (Pathfinder.Debug)
                Debug.Log(stringBuilder);
#endif
        }

        public void MoveTo()
        {
            if (_player.Level != Level) return;
            // remove last because it is the players current position
            if (_world.Path.Count > 0)
                _world.Path.RemoveAt(_world.Path.Count - 1);
            while (_world.Path.Count > 0)
            {
                var index = _world.Path.Count - 1;
                if (_player.TryMove(_world.Path[index]))
                    _world.Path.RemoveAt(index);
                else break;
            }
            _world.Path = null;
        }
    }
}
