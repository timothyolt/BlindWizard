using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ManageScores : MonoBehaviour {
    [SerializeField]
    private GameObject[] _kills, _turns, _shimmers, _blocks, _floors;

    private void Start () {
    }

    private void FixedUpdate ()
    {
        foreach (var kills in _kills)
            kills.GetComponent<TextMesh>().text = "Kills: " + Score.Kills;
        foreach (var turns in _turns)
            turns.GetComponent<TextMesh>().text = "Turns: " + Score.Turns;
        foreach (var blocks in _blocks)
            blocks.GetComponent<TextMesh>().text = "Blocks: " + Score.Blocks;
        foreach (var floors in _floors)
            floors.GetComponent<TextMesh>().text = "floors: " + Score.Floors;
        foreach (var shimmers in _shimmers)
            shimmers.GetComponent<TextMesh>().text = "shimmers: " + Score.Shimmers;

        //if (VrInputHelper.Secondary || VrInputHelper.Primary)
          //  SceneManager.LoadScene(0);
    }
}
