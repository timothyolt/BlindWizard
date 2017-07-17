using System;
using System.Linq;
using BlindWizard.Data;
using BlindWizard.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BlindWizard.MonoBehaviours
{
    public class EnemyMovement : MonoBehaviour
    {
        [SerializeField] private int _searchDistance = -1;
        public int SearchDistance
        {
            get => _searchDistance;
            set => _searchDistance = value;
        }

        [SerializeField] private RoomId _position;
        public RoomId Position
        {
            get => _position;
            set => _position = value;
        }

        [SerializeField] private int _level;
        public int Level
        {
            get => _level;
            set => _level = value;
        }
        
        [SerializeField] private PlayerMovement _player;
        public PlayerMovement Player
        {
            get => _player;
            set => _player = value;
        }
        
        [SerializeField] private World _world;
        public World World
        {
            get => _world;
            set
            {
                if (_world != null)
                    _world.Turn -= WorldOnTurn;
                value.Turn += WorldOnTurn;
                _world = value;
            }
        }

        private void Start()
        {
            if (World == null)
                World = FindObjectOfType<World>();
            else
                World.Turn += WorldOnTurn;
            if (Player == null)
                Player = FindObjectOfType<PlayerMovement>();
            if (SearchDistance < 0)
                SearchDistance = 4;
        }

        private void Update()
        {
            transform.position = World?.Levels[Level]?[Position]?
                                     .Container?.transform.position + Vector3.up * 0.75f ?? Vector3.zero;
        }

        private void WorldOnTurn(object sender, EventArgs eventArgs)
        {
            // Move to a random available diretion
            Position = Position[World.Levels[Level][Position].Walls.Open.Random()];
        }
    }
}