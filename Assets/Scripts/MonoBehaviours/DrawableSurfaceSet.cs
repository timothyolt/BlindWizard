using UnityEngine;

namespace BlindWizard.MonoBehaviours
{
    public class DrawableSurfaceSet : MonoBehaviour
    {
        [SerializeField]
        private GvrPointerPhysicsRaycaster _raycaster;
        public DrawableSurface Surface { get; set; }
        public bool Drawing { get; set; }

        private void Update()
        {
            if (!Drawing) return;
            //var laserpointerimpl = (GvrLaserPointerImpl)GvrPointerManager.Pointer;
            var ray = _raycaster.GetLastRay();
            RaycastHit hit;
            if (!Physics.Raycast(ray, out hit, 100)) return;
            if (hit.collider.gameObject == Surface?.gameObject)
                Surface?.DrawTo(hit.point);
        }
    }
}