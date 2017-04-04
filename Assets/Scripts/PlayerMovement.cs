using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    private bool _north, _south, _east, _west;
    public int PlayerX, PlayerZ;
    private int _level;

    [SerializeField] private Generator _manager;

    [SerializeField] private GameObject _camera;

    private void Start()
    {
        _level = 0;
        PlayerX = 0;
        PlayerZ = 0;
    }

    private void Update()
    {
        #region directions
        if (_camera.transform.rotation.eulerAngles.y >= 45 && _camera.transform.rotation.eulerAngles.y < 135)
        {
            //Debug.Log("West");
            _north = false;
            _south = false;
            _east = false;
            _west = true;
        }
        else if (_camera.transform.rotation.eulerAngles.y >= 135 && _camera.transform.rotation.eulerAngles.y < 225)
        {
            //Debug.Log("North");
            _north = true;
            _south = false;
            _east = false;
            _west = false;
        }
        else if (_camera.transform.rotation.eulerAngles.y >= 225 && _camera.transform.rotation.eulerAngles.y < 315)
        {
            //Debug.Log("East");
            _north = false;
            _south = false;
            _east = true;
            _west = false;
        }
        else if (_camera.transform.rotation.eulerAngles.y >= 315 || _camera.transform.rotation.eulerAngles.y < 45)
        {
            //Debug.Log("South");
            _north = false;
            _south = true;
            _east = false;
            _west = false;
        }
        #endregion

        #region NESW movement
        transform.position = _manager.Levels[_level].Rooms[PlayerX, PlayerZ].Container.transform.position;
        if (GvrController.AppButtonDown)
        {
            var room = _manager.Levels[_level].Rooms[PlayerX, PlayerZ];
            Debug.Log($"Walls N:{room.WallNorth == null} S:{room.WallSouth == null} E:{room.WallEast == null} W:{room.WallWest == null}");
            if (_north)
            {
                Debug.Log("App Button down north");
                if (_manager.Levels[_level].Rooms[PlayerX,PlayerZ].WallNorth == null)
                    PlayerZ--;
            }
            else if (_south)
            {
                Debug.Log("App Button down south");
                if (_manager.Levels[_level].Rooms[PlayerX, PlayerZ].WallSouth == null)
                    PlayerZ++;
            }
            else if (_east)
            {
                Debug.Log("App Button down east");
                if (_manager.Levels[_level].Rooms[PlayerX, PlayerZ].WallEast == null)
                    PlayerX--;
            }
            else if (_west)
            {
                Debug.Log("App Button down west");
                if (_manager.Levels[_level].Rooms[PlayerX, PlayerZ].WallWest == null)
                    PlayerX++;
            }
        }
        #endregion
        #region LevelMovement
        if(_manager.Levels[_level].Rooms[PlayerX, PlayerZ].Floor == null)
        {
           _level++;
           PlayerX++;
           PlayerZ++;
           _manager.Levels[_level - 1] = null;
           _manager.AddLevel(_level + 4);
        }
        #endregion
    }
}
