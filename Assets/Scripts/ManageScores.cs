using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ManageScores : MonoBehaviour {
    [SerializeField]
    private Text[] _kills, _turns, _shimmers, _blocks, _floors;

    private void Start () {
    }

    private void Update ()
    {
        foreach (var kills in _kills)
            kills.text = "Kills: " + Score.Kills;
        foreach (var turns in _turns)
            turns.text = "Turns: " + Score.Turns;
        foreach (var blocks in _blocks)
            blocks.text = "Blocks: " + Score.Blocks;
        foreach (var floors in _floors)
            floors.text = "floors: " + Score.Floors;
        foreach (var shimmers in _shimmers)
            shimmers.text = "shimmers: " + Score.Shimmers;

        if (VrInputHelper.Primary)
            SceneManager.LoadScene(0);
    }
}
