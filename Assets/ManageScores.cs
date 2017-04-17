using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManageScores : MonoBehaviour {
    private Scoreholding scores;
    public Text kills, turns, shimmers, blocks, floors;
	void Start () {
        scores = GameObject.Find("Scoreholder").GetComponent<Scoreholding>();
    }
	
	void Update () {

        
        kills = GameObject.FindGameObjectWithTag("kills").GetComponent<Text>();
        turns = GameObject.Find("turns").GetComponent<Text>();
        blocks = GameObject.FindGameObjectWithTag("blocks").GetComponent<Text>();
        floors = GameObject.Find("floors").GetComponent<Text>();
        shimmers = GameObject.Find("shimmers").GetComponent<Text>();

        kills.text = "Kills: ";
        turns.text = "Turns: ";
        blocks.text = "Blocks: ";
        floors.text = "floors: ";
        shimmers.text = "shimmers: ";

        /*if (scores != null)
        {
            kills.text = "Kills: " + scores.kills;
            turns.text = "Turns: " + scores.turns;
            blocks.text = "Blocks: " + scores.blocks;
            floors.text = "floors: " + scores.floors;
            shimmers.text = "shimmers: " + scores.shimmers;
        }
        else
        {
            kills.text = "Scores not found!";
        }*/
    }

    void Display()
    {
    }
}
