using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BlindWizard.Data;
using BlindWizard.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;
using Action = BlindWizard.Interfaces.Action;

namespace BlindWizard.MonoBehaviours
{
    public class World : MonoBehaviour, IActorWorld
    {
        //Prefabs
        [SerializeField]
        private GameObject _floorPrefab, _pitPrefab, _wallPrefab, _shimmerPrefab, _enemyPrefab, _pathPrefab;

        [SerializeField] private DrawableSurfaceSet _set;
        
        #if UNITY_EDITOR || DEVELOPMENT_BUILD
        [SerializeField] private GameObject _devTextPrefab;
        [SerializeField] private bool _pathfinderDebug;
        #endif
        
        [SerializeField]
        private Mesh _pathStraight, _pathCorner, _pathEnd, _pathPit, _pathNone;

        [SerializeField] private PlayerMovement _player;

        private List<WizardLevel> _pendingLevels;
        public List<WizardLevel> Levels { get; } = new List<WizardLevel>();

        public event EventHandler Turn; 
        public virtual void OnTurn(object sender) => Turn?.Invoke(sender, EventArgs.Empty);

        private void Start()
        {
            Score.Clear();
            _pendingLevels = new List<WizardLevel>();
            for (var i = 0; i < 4; i++)
                AddLevel();
            UpdateTurn();
        }

        private IEnumerator InstantiateLevel(WizardLevel level)
        {
            _pendingLevels.Remove(level);
#if  DEVELOPMENT_BUILD || UNITY_EDITOR
            yield return StartCoroutine(level.Instantiate(_floorPrefab, _pitPrefab, _shimmerPrefab, _enemyPrefab, _wallPrefab, _pathPrefab, _set, _devTextPrefab));
#else
            yield return StartCoroutine(level.Instantiate(_floorPrefab, _pitPrefab, _shimmerPrefab, _enemyPrefab, _wallPrefab, _set, _pathPrefab));
#endif
            Levels[level.Level] = level;
            _player.UpdatePosition();
        }

        private void Update()
        {
            if (VrInputHelper.Secondary)
                SceneManager.LoadScene(1);
            for (var i = _pendingLevels.Count - 1; i >= 0; i--)
                if (_pendingLevels[i].IsDone && _pendingLevels[i].Level <= _player.Level + 1)
                    StartCoroutine(InstantiateLevel(_pendingLevels[i]));
        }

        public void AddLevel()
        {
            var level = new WizardLevel(Levels.Count);
            level.Generate();
            _pendingLevels.Add(level);
            Levels.Add(null);
        }

        private List<RoomId> _path;
        public List<RoomId> Path
        {
            get => _path;
            set
            {
                // Reset previous path
#if DEVELOPMENT_BUILD || UNITY_EDITOR
                Pathfinder.Debug = _pathfinderDebug;
                for (var x = 0; x < Levels[_player.Level].Width; x++)
                for (var z = 0; z < Levels[_player.Level].Width; z++)
                {
                    var path = Levels[_player.Level][x, z].Path;
                    var parent = Levels[_player.Level].Pathfinder.Parent(_player.Position, new RoomId(x, z));
                    if (parent == null || !Pathfinder.Debug)
                    {
                        path.GetComponent<MeshFilter>().mesh = _pathNone;
                        continue;
                    }
                    path.GetComponent<MeshFilter>().mesh = _pathEnd;
                    path.transform.rotation =
                        Quaternion.AngleAxis(parent.Value.Rotation(), Vector3.up);
                    path.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                }
#else
                if (_path != null)
                    foreach (var roomId in _path)
                        Levels[_player.Level][roomId].Path.GetComponent<MeshFilter>().mesh = _pathNone;
#endif
                if (value != null)
                {
                    if (value.Count > 0)
                    {
                        var beginPath = Levels[_player.Level][value[value.Count - 1]].Path;
                        beginPath.GetComponent<MeshFilter>().mesh = _pathStraight;
                        beginPath.transform.rotation =
                            Quaternion.AngleAxis(
                                value[Math.Max(0, value.Count - 2)].Orient(value[value.Count - 1]).Rotation(),
                                Vector3.up);
                        beginPath.transform.localScale = Vector3.one * (2f / 1.5f);
                    }
                    for (var i = 1; i < value.Count - 1; i++)
                    {
                        var next = value[i - 1].Orient(value[i]).Rotation();
                        var last = value[i + 1].Orient(value[i]).Rotation();
                        var diff = last - next;
                        var path = Levels[_player.Level][value[i]].Path;
                        if (Math.Abs(diff) == 180)
                        {
                            path.GetComponent<MeshFilter>().mesh = _pathStraight;
                            path.transform.rotation = Quaternion.AngleAxis(next, Vector3.up);
                        }
                        else
                        {
                            path.GetComponent<MeshFilter>().mesh = _pathCorner;
                            // Find which pairing creates the right angle
                            path.transform.rotation =
                                Quaternion.AngleAxis((next + 90) % 360 == last ? next : last, Vector3.up);
                        }
                        path.transform.localScale = Vector3.one * (2f / 1.5f);
                    }
                    if (value.Count > 1)
                    {
                        var endPath = Levels[_player.Level][value[0]].Path;
                        endPath.GetComponent<MeshFilter>().mesh =
                            Levels[_player.Level][value[0]].FloorGen ? _pathEnd : _pathPit;
                        endPath.transform.rotation =
                            Quaternion.AngleAxis(value[1].Orient(value[0]).Rotation(), Vector3.up);
                        endPath.transform.localScale = Vector3.one * (2f / 1.5f);
                    }
                }
                _path = value;
            }
        }
        
        private readonly List<IActor> _actorsAdd = new List<IActor>();
        private readonly List<IActor> _actorsRemove = new List<IActor>();
        private readonly Dictionary<IActor, Action?> _actors = new Dictionary<IActor, Action?>();

        private void UpdateTurn()
        {
            foreach (var actor in _actorsAdd)
                _actors.Add(actor, null);
            foreach (var actor in _actorsRemove)
                _actors.Remove(actor);
            foreach (var actor in _actors.Keys)
            {
                _actors[actor] = null;
                actor.GetAction(readyAction =>
                {
                    _actors[actor] = readyAction;
                    if (_actors.Values.All(action => action != null))
                        UpdateTurn();
                });
            }
        }

        public IEnumerable<IActor> Actors => _actors.Keys;

        public WizardLevel this[int level] => Levels[level];

        public int Count => Levels.Count;

        public void AddActor(IActor actor) => _actorsAdd.Add(actor);

        public void RemoveActor(IActor actor) => _actorsRemove.Add(actor);

        private void OnDestroy()
        {
            _actorsAdd.Clear();
            _actorsRemove.Clear();
            _actors.Clear();
        }
    }
}
