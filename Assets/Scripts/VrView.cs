using System.Collections;
using Gvr.Internal;
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
            StartCoroutine(LoadVrDevice(_stereo ? "android" : "android"));
        }
    }

    private IEnumerator LoadVrDevice(string platform)
    {
        switch (platform.ToLower())
        {
            case "android":
                VRSettings.LoadDeviceByName(new [] {"daydream", "cardboard"});
                yield return null;
                VRSettings.enabled = true;
                // Reset NoVr offset
                transform.parent.localRotation = Quaternion.Euler(0, 0, 0);
                transform.localPosition = new Vector3(0, 1, 0);
                // Enable any special GVR stuff
                if (_googleVrObjects != null)
                    foreach (var googleVrObject in _googleVrObjects)
                        googleVrObject?.SetActive(true);
                break;
            default:
                VRSettings.LoadDeviceByName("None");
                yield return null;
                VRSettings.enabled = false;
                Input.gyro.enabled = true;
                Screen.autorotateToPortraitUpsideDown = false;
                Screen.autorotateToPortrait = false;
                // Set NoVr offset
                // TODO (timothyolt): settings adjustable offset. Idk for dorks who play laying down.
                transform.parent.localRotation = Quaternion.Euler(90, 0, 0);
                transform.localPosition = new Vector3(0, 0, -1);
                // Disable any special GVR stuff
                if (_googleVrObjects != null)
                    foreach (var googleVrObject in _googleVrObjects)
                        googleVrObject?.SetActive(false);
                break;
        }
    }

    private void Start ()
    {
        // TODO (timothyolt): settings override
        Debug.Log("yoyoyo " + GvrController.State);
        Stereo = GvrIntent.IsLaunchedFromVr();
        Debug.Log("yoyoyo " + GvrController.State);
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
