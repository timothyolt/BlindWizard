using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    public int Level { get; private set; }

    private Vector2 _position;
    public Vector2 Position
    {
        get => _position;
        set
        {
            _position = value;
            var room = _generator.Levels[Level]?.Rooms[(int) _position.x, (int) _position.y];
            if (room == null) return;
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
                _generator.Levels[Level - 1].Destroy();
                _generator.AddLevel();
            }
            UpdatePosition();
        }
    }

    public void UpdatePosition()
    {
        // 1.6f is a GoogleVR constant.
        if (_generator.Levels.Count > Level && _generator.Levels[Level] != null)
            transform.position = _generator.Levels[Level]
                                     .Rooms[(int) _position.x, (int) _position.y]
                                     .Container.transform.position + Vector3.up * 1.6f;
    }

    [SerializeField] private Generator _generator;

    private void Start()
    {
        Level = 0;
        Position = Vector2.zero;
    }

    private void Update()
    {
       UpdatePosition();
    }
}
