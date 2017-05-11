using System;
using System.Collections;
using UnityEngine;
using UnityEngine.VR;

public class VrView : MonoBehaviour
{
    #if UNITY_EDITOR
    public enum EditorViewType
    {
        Daydream, Cardboard
    }

    public EditorViewType ViewType;
    #endif

    [SerializeField]
    private GameObject[] _googleCardboardObjects;

    [SerializeField]
    private bool _stereo;
    public bool Stereo
    {
        get => _stereo;
        set
        {
            _stereo = value;
            // StartCoroutine(LoadVrDevice(_stereo ? "GoogleVR" : "none"));
            StartCoroutine(LoadVrDevice("GoogleVr"));
        }
    }

    private IEnumerator LoadVrDevice(string platform)
    {
        switch (platform.ToLower())
        {
            case "googlevr":
                // VRSettings.LoadDeviceByName(new [] {"daydream", "cardboard"});
                // yield return null;
                // VRSettings.enabled = true;
                // Reset NoVr offset
                transform.parent.localRotation = Quaternion.Euler(0, 0, 0);
                //transform.localPosition = new Vector3(0, 0, 0);
                // Enable any special GVR stuff
                if (_googleCardboardObjects != null)
                    foreach (var googleVrObject in _googleCardboardObjects)
                        googleVrObject?.SetActive(true);
                break;
            default:
                VRSettings.LoadDeviceByName("None");
                yield return null;
                VRSettings.enabled = false;
                Input.gyro.enabled = true;
                Screen.orientation = ScreenOrientation.Landscape;
                // Set NoVr offset
                // TODO (timothyolt): settings adjustable offset. Idk for dorks who play laying down.
                transform.parent.localRotation = Quaternion.Euler(90, 0, 0);
                //transform.localPosition = new Vector3(0, 0, -1);
                // Disable any special GVR stuff
                if (_googleCardboardObjects != null)
                    foreach (var googleVrObject in _googleCardboardObjects)
                        googleVrObject?.SetActive(false);
                break;
        }
    }

    private void Start ()
    {
        // TODO (timothyolt): settings override
        Debug.Log($"yoyoyo {VRDevice.model}");
        Stereo = true;  // GvrIntent.IsLaunchedFromVr();
    }

    private void Update()
    {
        switch (GvrController.ApiStatus)
        {
            case GvrControllerApiStatus.Ok:
                if (_googleCardboardObjects != null)
                    foreach (var googleVrObject in _googleCardboardObjects)
                        googleVrObject?.SetActive(false);
                break;
            case GvrControllerApiStatus.Error:
            case GvrControllerApiStatus.Unsupported:
            case GvrControllerApiStatus.NotAuthorized:
            case GvrControllerApiStatus.Unavailable:
            case GvrControllerApiStatus.ApiServiceObsolete:
            case GvrControllerApiStatus.ApiClientObsolete:
            case GvrControllerApiStatus.ApiMalfunction:
                if (_googleCardboardObjects != null)
                    foreach (var googleVrObject in _googleCardboardObjects)
                        googleVrObject?.SetActive(true);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        // Debug.Log($"{GvrController.ApiStatus} {GvrController.State}");
    }

//    private void Update ()
//    {
//        if (!Stereo)
//        {
//            transform.localPosition = new Vector3(0, 0, -1);
//            transform.localRotation = new Quaternion(
//                Input.gyro.attitude.x, Input.gyro.attitude.y, -Input.gyro.attitude.z, -Input.gyro.attitude.w);
//        }
//    }
}
