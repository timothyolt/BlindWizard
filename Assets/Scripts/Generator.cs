using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Generator : MonoBehaviour
{
    //Prefabs
    [SerializeField]
    private GameObject _floorPrefab, _wallPrefab, _shimmerPrefab, _enemyPrefab;

    public List<WizardLevel> Levels { get; } = new List<WizardLevel>();

    private void Start()
    {
        Score.Clear();
        for (var i = 1; i <= 4; i++)
            AddLevel(i);
    }

    public void AddLevel(int level)
    {
        var wizardLevel = new WizardLevel(level);
        wizardLevel.Instantiate(_floorPrefab, _shimmerPrefab, _enemyPrefab, _wallPrefab);
        Levels.Add(wizardLevel);
    }

    private void Update()
    {
        if (VrInputHelper.Secondary)
            SceneManager.LoadScene(1);
    }
}
