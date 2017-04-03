using System;
using UnityEngine;

public class WizWave : MonoBehaviour
{
    [SerializeField]
    private float time;

    [SerializeField]
    private Light spotlight;
    [SerializeField]
    private float angle;
    [SerializeField]
    private float distance;
    [SerializeField]
    private float travelRate;

	// Use this for initialization
	void Start ()
	{
	    time = 0;
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (spotlight == null) return;
	    if (time < distance)
	    {
	        spotlight.spotAngle = angle;
	        spotlight.range = time;
	    }
	    else if (time < distance * 2)
	    {
	        var opposite = Math.Tan(Math.PI * (angle / 2d) / 180d) * distance;
	        var rad = Math.Atan(opposite / (distance - time) * -1d);
	        spotlight.spotAngle = (float) (rad * (180d / Math.PI));
	        spotlight.range = distance;
	    }
	    else time -= distance * 2;
	    time += Time.deltaTime * travelRate;
	}
}
