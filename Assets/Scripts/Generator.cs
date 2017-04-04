using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Generator : MonoBehaviour {

	//Prefabs
    [SerializeField]
	private GameObject _floor, _wall, _shimmer, _enemy;

    public List<WizardLevel> FinalLevels { get; } = new List<WizardLevel>();

    private void Start ()
	{
	    for (var i = 1; i < 25; i++)
	        FinalLevels.Add(new WizardLevel(i, _floor, _wall, _shimmer, _enemy));
	}


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            SceneManager.LoadScene(0);
    }
}