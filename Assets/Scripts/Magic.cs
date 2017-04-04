using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic : MonoBehaviour {
    PlayerMovement player;
    Generator manager;
    public GameObject fire;
    public int health = 5;
    // Use this for initialization
    void Start () {
        manager = GameObject.Find("Manager").GetComponent<Generator>();
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void blink()
    {
        if(player.north && manager.levels[player.level].Rooms[player.playerx, player.playerz].WallNorth != null && manager.levels[player.level].Rooms[player.playerx, player.playerz+1].WallNorth != null)
        {
            player.playerz += 2;
        }
        else if (player.east && manager.levels[player.level].Rooms[player.playerx, player.playerz].WallEast != null && manager.levels[player.level].Rooms[player.playerx+1, player.playerz].WallEast != null)
        {
            player.playerx += 2;
        }
        else if (player.west && manager.levels[player.level].Rooms[player.playerx, player.playerz].WallWest != null && manager.levels[player.level].Rooms[player.playerx-1, player.playerz].WallWest != null)
        {
            player.playerx -= 2;
        }
        else if (player.south && manager.levels[player.level].Rooms[player.playerx, player.playerz].WallSouth != null && manager.levels[player.level].Rooms[player.playerx, player.playerz-1].WallSouth != null)
        {
            player.playerz -= 2;
        }

    }

    void heal()
    {
        health++;
    }

    void fireball()
    {

    }

    void wizWave()
    {

    }

}
