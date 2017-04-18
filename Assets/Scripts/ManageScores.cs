using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ManageScores : MonoBehaviour {
    [SerializeField]
    private Text _kills, _turns, _shimmers, _blocks, _floors;

    private void Start () {
    }

    private void Update () {
        _kills.text = "Kills: " + Score.Kills;
        _turns.text = "Turns: " + Score.Turns;
        _blocks.text = "Blocks: " + Score.Blocks;
        _floors.text = "floors: " + Score.Floors;
        _shimmers.text = "shimmers: " + Score.Shimmers;

        if (GvrController.AppButtonDown)
            SceneManager.LoadScene(0);
    }
}
