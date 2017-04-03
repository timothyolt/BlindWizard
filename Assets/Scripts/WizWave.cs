using System;
using UnityEngine;

public class WizWave : MonoBehaviour
{
    [SerializeField]
    private float time;

    [SerializeField]
    private Light _spotlight;
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
	    if (_spotlight == null) return;
	    if (time < distance)
	    {
	        _spotlight.spotAngle = angle;
	        _spotlight.range = time;
	    }
	    else if (time < distance * 2)
	    {
	        var opposite = Math.Tan(Math.PI * (angle / 2d) / 180d) * distance;
	        var rad = Math.Atan(opposite / (distance - time) * -1d);
	        _spotlight.spotAngle = (float) (rad * (180d / Math.PI));
	        _spotlight.range = distance;
	    }
	    else time -= distance * 2;
	    time += Time.deltaTime * travelRate;
	}
}
