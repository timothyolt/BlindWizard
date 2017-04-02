using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {

    private static (string, string) Foo()
    {
        return ("your", "mother");
    }

	// Use this for initialization
	void Start ()
	{
	    var ym = Foo();
	}
	
	// Update is called once per frame
	void Update () {
	    var ym = Foo();
	    Console.Write(ym.Item1);
	}
}
