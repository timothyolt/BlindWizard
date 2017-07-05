using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BlindWizard.MonoBehaviours
{
    public class DrawableSurface : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        private Material _material;
        private Texture2D _texture;

        [SerializeField] private int _width, _height;

        private PointerEventData _begin, _previous, _end;

        [SerializeField] private DrawableSurfaceSet _set;
        
        public DrawableSurfaceSet Set
        {
            get => _set;
            set => _set = value;
        }

        private void Start()
        {
            _material = GetComponent<Renderer>().material;
        }

        private Texture2D Texture
        {
            get
            {
                if (_texture != null) return _texture;
                _texture = Instantiate(_material.mainTexture) as Texture2D;
                _material.mainTexture = _texture;
                return _texture;
            }
        }

        public void DrawTo(Vector3 to)
        {
            
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            for (var x = 1024; x < 1024 + 32; x++)
            for (var y = 1024; y < 1024 + 32; y++)
                Texture.SetPixel(x, y, Color.green);
            Texture.Apply();
            Texture
            
            Set.Drawing = true;
            Set.Surface = this;
            _begin = eventData;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Set.Drawing = false;
            _end = eventData;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _begin = eventData;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            // TODO: track non-surface pointer up events
            _end = eventData;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("Click");
        }
    }
}