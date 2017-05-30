using System;
using System.Collections;
using System.Collections.Generic;
using Blindwizard.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Blindwizard.MonoBehaviours
{
    public class World : MonoBehaviour
    {
        //Prefabs
        [SerializeField]
        private GameObject _floorPrefab, _pitPrefab, _wallPrefab, _shimmerPrefab, _enemyPrefab, _pathPrefab;
        
        [SerializeField]
        private Mesh _pathStraight, _pathCorner, _pathEnd, _pathPit, _pathNone;

        [SerializeField] private PlayerMovement _player;

        private List<WizardLevel> _pendingLevels;
        public List<WizardLevel> Levels { get; } = new List<WizardLevel>();

        private void Start()
        {
            Score.Clear();
            _pendingLevels = new List<WizardLevel>();
            for (var i = 0; i < 4; i++)
                AddLevel();
        }

        private IEnumerator InstantiateLevel(WizardLevel level)
        {
            _pendingLevels.Remove(level);
            yield return StartCoroutine(level.Instantiate(_floorPrefab, _pitPrefab, _shimmerPrefab, _enemyPrefab, _wallPrefab, _pathPrefab));
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
            _pendingLevels.Add(new WizardLevel(Levels.Count));
            Levels.Add(null);
        }

        private List<RoomId> _path;
        public List<RoomId> Path
        {
            get => _path;
            set
            {
                if (_path != null)
                    foreach (var roomId in _path)
                        Levels[_player.Level][roomId].Path.GetComponent<MeshFilter>().mesh = _pathNone;
                if (value != null)
                {
                    if (value.Count > 1)
                    {
                        var endPath = Levels[_player.Level][value[0]].Path;
                        endPath.GetComponent<MeshFilter>().mesh = _pathEnd;
                        endPath.transform.rotation =
                            Quaternion.AngleAxis(value[1].Orient(value[0]).Rotation(), Vector3.up);
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
                            path.transform.rotation = Quaternion.AngleAxis((next + 90) % 360 == last ? next : last, Vector3.up);
                        }
                    }
                    if (value.Count > 0)
                    {
                        var beginPath = Levels[_player.Level][value[value.Count - 1]].Path;
                        beginPath.GetComponent<MeshFilter>().mesh = _pathStraight;
                        beginPath.transform.rotation =
                            Quaternion.AngleAxis(
                                value[Math.Max(0, value.Count - 1)].Orient(value[value.Count - 1]).Rotation(),
                                Vector3.up);
                    }
                }
                _path = value;
            }
        }
    }
}
