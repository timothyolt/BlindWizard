using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour {

    private void Start () {
		
	}

    private void Update () {
		
	}

    public void SetGazedAt(bool gazed)
    {
        Debug.Log($"Gazed at menu button {gazed}");
    }

    public void ShowMenu()
    {
        SceneManager.LoadScene(1);
    }
}
