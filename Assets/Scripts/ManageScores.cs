using UnityEngine;
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
    }

    /// <summary>
    /// I'll let you guess what this does.
    /// </summary>
    public void ReturnToMain()
    {
        //code for returning to main menu
    }
}
