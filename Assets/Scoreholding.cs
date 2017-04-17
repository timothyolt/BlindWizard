using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoreholding : MonoBehaviour {

    public int blocks = 0, floors = 0, kills = 0, shimmers = 0, turns = 0;

	void Start () {
        DontDestroyOnLoad(transform.gameObject);
    }
/// <summary>
/// increases the number of blocks the player has traveled
/// </summary>
    public void BlockUp()
    {
        blocks++;
    }
/// <summary>
/// increases the number of floors the player has traveled
/// </summary>
    public void floorUp()
    {
        floors++;
    }
    /// <summary>
    /// increses the number of enemies the player has killed
    /// </summary>
    public void KillsUp()
    {
        kills++;
    }
    /// <summary>
    /// increases the number of shimmers the player has collected
    /// </summary>
    public void ShimmersUp()
    {
        shimmers++;
    }
    /// <summary>
    /// increases the number of turns the player has taken
    /// </summary>
    public void Turnip()
    {
        turns++;
    }
}
