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
            _world.Path = null;
            if (_player.Level == Level)
                _player.Position = Position;
        }
    }
}
