using System.Net.Configuration;
using UnityEngine;

public class Room
{
    public Room()
    {
        HasWallNorth = true;
        HasWallSouth = true;
        HasWallEast = true;
        HasWallWest = true;
    }

    public bool IsGenerated { get; set; }
    public bool HasFloor { get; set; }
    public bool HasWallNorth { get; set; }
    public bool HasWallSouth { get; set; }
    public bool HasWallEast { get; set; }
    public bool HasWallWest { get; set; }
    public bool HasEnemy { get; set; }
    public bool HasShimmer { get; set; }
    /// <summary>
    /// GameObject that always exists within a room
    /// </summary>
    public GameObject Container { get; set; }
    /// <summary>
    /// Floor GameObject of a room, will be null if this is a "pit room"
    /// </summary>
    public GameObject Floor { get; set; }
    /// <summary>
    /// GameObject for the Northern Wall, will be null if there is no wall
    /// </summary>
    public GameObject WallNorth { get; set; }
    /// <summary>
    /// GameObject for South Wall, will be null if there is no wall
    /// </summary>
    public GameObject WallSouth { get; set; }
    /// <summary>
    /// GameObject for the Eastern Wall, will be null if there is no wall
    /// </summary>
    public GameObject WallEast { get; set; }
    /// <summary>
    /// GameObject for the Western Wall, will be null if there is no wall
    /// </summary>
    public GameObject WallWest { get; set; }
    /// <summary>
    /// GameObject for an enemy in the room. Will be null if there is no enemy in the room
    /// </summary>
    public GameObject Enemy { get; set; }
    /// <summary>
    /// GameObject for a Shimmer in the room. Will be null if there is no shimmer
    /// </summary>
    public GameObject Shimmer { get; set; }
}
