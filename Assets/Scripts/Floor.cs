using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Floor : MonoBehaviour
{

    private PlayerMovement _player;
    [SerializeField]
    private Material _gazedMaterial, _ungazedMaterial;
    private MeshRenderer _renderer;

    public Vector2 Position { get; set; }
    public int Level { get; set; }

    private void Start ()
    {
        _player = FindObjectOfType<PlayerMovement>();
        _renderer = GetComponent<MeshRenderer>();
    }

    public void SetGazedAt(bool gazed)
    {
        //Debug.Log($"Gazed at {Position.x},{Position.y} {gazed}");
        _renderer.material = gazed ? _gazedMaterial : _ungazedMaterial;
    }

    public void MoveTo()
    {
        if (_player.Level == Level)
            _player.Position = Position;
    }
}
