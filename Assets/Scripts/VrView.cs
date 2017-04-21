using System.Security.AccessControl;
using UnityEngine;

public class VrView : MonoBehaviour {

	// Use this for initialization
    private void Start ()
    {
        Input.gyro.enabled = true;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.autorotateToPortrait = false;
    }
	
	// Update is called once per frame
    private void Update ()
    {
        transform.localRotation = new Quaternion(
                Input.gyro.attitude.x, Input.gyro.attitude.y, -Input.gyro.attitude.z, -Input.gyro.attitude.w);
        Debug.Log(Input.gyro.attitude.eulerAngles);
    }
}
