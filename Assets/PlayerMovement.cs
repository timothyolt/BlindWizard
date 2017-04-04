using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    public bool north, south, east, west;
    public int playerx, playerz, level;
    Generator manager;
    // Use this for initialization
    void Start()
    {
        manager = GameObject.Find("Manager").GetComponent<Generator>();
        level = 0;
        playerx = 0;
        playerz = 0;
    }
    // Update is called once per frame
    void Update()
    {
        #region directions
        if (transform.rotation.y >= 45 && transform.rotation.y < 135)
        {
            north = true;
            south = false;
            east = false;
            west = false;
        }
        else if (transform.rotation.y >= 135 && transform.rotation.y < 225)
        {
            north = false;
            south = false;
            east = false;
            west = true;
        }
        else if (transform.rotation.y >= 225 && transform.rotation.y < 315)
        {
            north = false;
            south = true;
            east = false;
            west = false;
        }
        else if (transform.rotation.y >= 315 && transform.rotation.y < 45)
        {
            north = false;
            south = false;
            east = true;
            west = false;
        }
        #endregion

        #region NESW movement
        gameObject.transform.position = manager.levels[level].Rooms[playerx, playerz].Container.transform.position;
        if (GvrController.AppButtonDown && north)
        {
            if (manager.levels[level].Rooms[playerx,playerz].WallNorth != null)
                playerz++;
        }
        else if (GvrController.AppButtonDown && south)
        {
            if (manager.levels[level].Rooms[playerx, playerz].WallSouth != null)
                playerz--;
        }
        else if (GvrController.AppButtonDown && east)
        {
            if (manager.levels[level].Rooms[playerx, playerz].WallEast != null)
                playerx++;
        }
        else if (GvrController.AppButtonDown && west)
        {
            if (manager.levels[level].Rooms[playerx, playerz].WallWest != null)
                playerx--;
        }
        #endregion
        #region LevelMovement
        if(manager.levels[level].Rooms[playerx, playerz].Floor == null)
        {
           level++;
           manager.WorkTheMagic();
        }
        #endregion
    }
}
