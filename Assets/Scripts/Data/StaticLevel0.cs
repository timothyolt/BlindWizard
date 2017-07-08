using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticLevel0 : BlindWizard.Data.WizardLevel {
    public StaticLevel0(int level) : base(level)
    {
    }

   
    protected override void GenerateMaze()
    {
        // no maze test
    }

    public override void Generate()
    {
        GenerateRooms();
        // oposite corner has hole
        Rooms[3, 3].FloorGen = false;

        // other 2 corners have shimmers
        Rooms[0, 3].ShimmerGen = true;
        Rooms[3, 0].ShimmerGen = true;

        // L shaped walls leading towards pit
        GenerateWallsLevel0();

        GenerateMaze();
        
    }
    public override bool IsDone
    {
        get
        {
            return true;
        }
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
