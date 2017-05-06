using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Generator : MonoBehaviour
{
    //Prefabs
    [SerializeField]
    private GameObject _floorPrefab, _pitPrefab, _wallPrefab, _shimmerPrefab, _enemyPrefab;

    [SerializeField] private PlayerMovement _player;

    private List<WizardLevel> _pendingLevels;
    public List<WizardLevel> Levels { get; } = new List<WizardLevel>();

    private void Start()
    {
        Score.Clear();
        _pendingLevels = new List<WizardLevel>();
        for (var i = 0; i < 2; i++)
            AddLevel();
    }

    public void AddLevel()
    {
        _pendingLevels.Add(new WizardLevel(Levels.Count));
        Levels.Add(null);
    }

    private IEnumerator InstantiateLevel(WizardLevel level)
    {
        _pendingLevels.Remove(level);
        yield return StartCoroutine(level.Instantiate(_floorPrefab, _pitPrefab, _shimmerPrefab, _enemyPrefab, _wallPrefab));
        Levels[level.Level] = level;
        _player.UpdatePosition();
    }

    private void Update()
    {
        if (VrInputHelper.Secondary)
            SceneManager.LoadScene(1);
        for (var i = _pendingLevels.Count - 1; i >= 0; i--)
            if (_pendingLevels[i].IsDone)
                StartCoroutine(InstantiateLevel(_pendingLevels[i]));
    }
}
