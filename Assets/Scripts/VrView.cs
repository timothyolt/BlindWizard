using UnityEngine;
using UnityEngine.VR;

public class VrView : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _googleVrObjects;

    [SerializeField]
    private bool _stereo;
    public bool Stereo
    {
        get => _stereo;
        set
        {
            _stereo = value;
            if (_stereo) {
                VRSettings.LoadDeviceByName("daydream");
                VRSettings.enabled = true;
                // Reset NoVr offset
                transform.parent.localRotation = Quaternion.Euler(0, 0, 0);
                transform.localPosition = new Vector3(0, 2, 0);
                // Enable any special GVR stuff
                if (_googleVrObjects == null) return;
                foreach (var googleVrObject in _googleVrObjects)
                    googleVrObject?.SetActive(true);
            }
            else
            {
                VRSettings.LoadDeviceByName("None");
                VRSettings.enabled = false;
                Input.gyro.enabled = true;
                Screen.autorotateToPortraitUpsideDown = false;
                Screen.autorotateToPortrait = false;
                // Set NoVr offset
                // TODO (timothyolt): settings adjustable offset. Idk for dorks who play laying down.
                transform.parent.localRotation = Quaternion.Euler(90, 0, 0);
                transform.localPosition = new Vector3(0, 0, -1);
                // Disable any special GVR stuff
                if (_googleVrObjects == null) return;
                foreach (var googleVrObject in _googleVrObjects)
                    googleVrObject?.SetActive(false);
            }
        }
    }

    private void Start ()
    {
        // TODO (timothyolt): settings override
        Debug.Log("yoyoyo " + GvrIntent.GetData());
        Stereo = GvrIntent.IsLaunchedFromVr();
    }

    private void Update ()
    {
        if (!Stereo)
        {
            transform.localPosition = new Vector3(0, 0, -1);
            transform.localRotation = new Quaternion(
                Input.gyro.attitude.x, Input.gyro.attitude.y, -Input.gyro.attitude.z, -Input.gyro.attitude.w);
        }
        else
            transform.localPosition = new Vector3(0, 1, 0);
    }
}
