using System;
using System.Collections;
using UnityEngine;
using UnityEngine.VR;

namespace BlindWizard.MonoBehaviours
{
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
        private GameObject[] _googleCardboardObjects, _googleDaydreamObjects;

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
                    //if (_googleCardboardObjects != null)
                    //    foreach (var googleVrObject in _googleCardboardObjects)
                    //        googleVrObject?.SetActive(true);
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
            //Stereo = true;  // GvrIntent.IsLaunchedFromVr();
        }

        private void Update()
        {
#if UNITY_ANDROID
            switch (GvrController.State)
            {
                case GvrConnectionState.Error:
                case GvrConnectionState.Disconnected:
                case GvrConnectionState.Scanning:
                case GvrConnectionState.Connecting:
                    if (_googleCardboardObjects != null)
                        foreach (var googleVrObject in _googleCardboardObjects)
                            googleVrObject?.SetActive(true);
                    if (_googleDaydreamObjects != null)
                        foreach (var googleVrObject in _googleDaydreamObjects)
                            googleVrObject?.SetActive(false);
                    break;
                case GvrConnectionState.Connected:
                    if (_googleCardboardObjects != null)
                        foreach (var googleVrObject in _googleCardboardObjects)
                            googleVrObject?.SetActive(false);
                    if (_googleDaydreamObjects != null)
                        foreach (var googleVrObject in _googleDaydreamObjects)
                            googleVrObject?.SetActive(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
#else
		if (_googleCardboardObjects != null)
			foreach (var googleVrObject in _googleCardboardObjects)
				googleVrObject?.SetActive(true);
		if (_googleDaydreamObjects != null)
			foreach (var googleVrObject in _googleDaydreamObjects)
				googleVrObject?.SetActive(false);
#endif
#if DEBUG
            Debug.Log($"{GvrController.ApiStatus} {GvrController.State}");
#endif
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
}
