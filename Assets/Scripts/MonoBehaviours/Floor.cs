using System.Text;
using Blindwizard.Data;
using UnityEngine;

namespace Blindwizard.MonoBehaviours
{
    [RequireComponent(typeof(MeshRenderer))]
    public class Floor : MonoBehaviour
    {
        private World _world;
        private PlayerMovement _player;
        [SerializeField]
        private Material _gazedMaterial, _ungazedMaterial;
        private MeshRenderer _renderer;

        public RoomId Position { get; set; }
        public int Level { get; set; }

        private void Start ()
        {
            _world = FindObjectOfType<World>();
            _player = FindObjectOfType<PlayerMovement>();
            _renderer = GetComponent<MeshRenderer>();
        }

        public void SetGazedAt(bool gazed)
        {
            //Debug.Log($"Gazed at {Position.x},{Position.y} {gazed}");
            _renderer.material = gazed ? _gazedMaterial : _ungazedMaterial;
            if (gazed)
            {
                var path = _world.Levels[Level].Pathfinder.Shortest(_player.Position, Position);
                var stringBuilder = new StringBuilder();
                foreach (var roomId in path)
                    stringBuilder.Append($"{roomId.X}, {roomId.Z} : ");
                #if DEBUG
                Debug.Log(stringBuilder);
                #endif
            }
        }

        public void MoveTo()
        {
            if (_player.Level == Level)
                _player.Position = Position;
        }
    }
}
