using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    //private bool _north, _south, _east, _west;
    public int Level { get; private set; }

    private Vector2 _position;
    public Vector2 Position
    {
        get => _position;
        set
        {
            _position = value;
            var room = _generator.Levels[Level].Rooms[(int) _position.x, (int) _position.y];
            Score.Turnip();
            if (room.Shimmer != null)
            {
                Destroy(room.Shimmer);
                Score.ShimmersUp();
            }
            if (room.Floor == null)
            {
                Score.FloorUp();
                Level++;
                _position += new Vector2(1, 1);
                _generator.Levels[Level - 1].Destroy(); //This code ain't work lululul
                _generator.AddLevel(Level + 4);
            }
            // 1.6f is a GoogleVR constant.
            transform.position = _generator.Levels[Level]
                                     .Rooms[(int) _position.x, (int) _position.y]
                                     .Container.transform.position + Vector3.up * 1.6f;
        }
    }

    [SerializeField] private Generator _generator;

    [SerializeField] private GameObject _camera;

    private void Start()
    {
        Level = 0;
        Position = Vector2.zero;
    }

 /*   private void Update()
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
    }*/
}
