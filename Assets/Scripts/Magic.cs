﻿using UnityEngine;

public class Magic : MonoBehaviour {
    private PlayerMovement _player;
    private Manager _manager;
    public GameObject Fire;
    public int Health = 5;
    // Use this for initialization
    private void Start () {
        _manager = GameObject.Find("Manager").GetComponent<Manager>();
        _player = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }
	
	// Update is called once per frame
    private void Update () {
		
	}

    void Blink()
    {

/*
        if(_player.north && _manager.levels[_player.level].Rooms[_player.playerx, _player.playerz].WallNorth != null && _manager.levels[_player.level].Rooms[_player.playerx, _player.playerz+1].WallNorth != null)
        {
            _player.playerz += 2;
        }
        else if (_player.east && _manager.levels[_player.level].Rooms[_player.playerx, _player.playerz].WallEast != null && _manager.levels[_player.level].Rooms[_player.playerx+1, _player.playerz].WallEast != null)
        {
            _player.playerx += 2;
        }
        else if (_player.west && _manager.levels[_player.level].Rooms[_player.playerx, _player.playerz].WallWest != null && _manager.levels[_player.level].Rooms[_player.playerx-1, _player.playerz].WallWest != null)
        {
            _player.playerx -= 2;
        }
        else if (_player.south && _manager.levels[_player.level].Rooms[_player.playerx, _player.playerz].WallSouth != null && _manager.levels[_player.level].Rooms[_player.playerx, _player.playerz-1].WallSouth != null)
        {
            _player.playerz -= 2;
        }
*/

    }

    void heal()
    {
        Health++;
    }

    void fireball()
    {

    }

    void wizWave()
    {

    }

}
