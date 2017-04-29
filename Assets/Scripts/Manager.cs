﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
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
        => Levels.Add(new WizardLevel(level, _floorPrefab, _wallPrefab, _shimmerPrefab, _enemyPrefab));

    private void Update()
    {
        if (VrInputHelper.Primary)
            SceneManager.LoadScene(1);
    }
}
