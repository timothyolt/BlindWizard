using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Generator : MonoBehaviour {

	//Prefabs
	public GameObject empty, tileCube, shimmer, enemy, wallVertical, wallHorizontal;


	//Arrays that store waypoint coordinates
	public GameObject[,] fourxFourArray = new GameObject[5,5];
	public GameObject[,] sixxSixArray = new GameObject[7,7];
	public GameObject[,] eightxEightArray = new GameObject[9,9];

	void Start () 
	{
	    var level = new WizardLevel(1, tileCube, wallHorizontal, shimmer, enemy);
		WorkTheMagic();
	}
	

	void Update () 
	{
		
	}

	//Works the magic.
	void WorkTheMagic()
	{
//	    BuildFloor(4);
//		SixxSix();
//		EightxEight();
//		PlaceWalls();
//		PlaceShimmers();
//		PlaceEnemies();
	}


	void PlaceWalls()
	{
		GameObject[] waypoints = GameObject.FindGameObjectsWithTag("waypoint");

		foreach (GameObject point in waypoints)
		{
			int tester = Random.Range (0,4);
			if (tester == 3)
			{
				int tester2 = Random.Range(0,2);

				if (tester2 == 1)
				{
					GameObject newWall = Instantiate(wallVertical);
					newWall.transform.position = new Vector3(point.transform.position.x + .5f, point.transform.position.y, point.transform.position.z);
				}
				else 
				{
					GameObject newWall = Instantiate(wallHorizontal);
					newWall.transform.position = new Vector3(point.transform.position.x, point.transform.position.y, point.transform.position.z + .5f);
				}
			}
		}
	}

	void PlaceShimmers()
	{
		int fourxFourLimiter = 0;
		int sixxSixLimiter = 0;
		int eightxEightLimiter = 0;
		int upperbound4 = 3;
		int upperbound6 = 3;
		int upperbound8 = 3;

		foreach(GameObject waypoint in fourxFourArray)
		{
			
			int tester = Random.Range(0, upperbound4);
			if (tester == 2 && fourxFourLimiter < 2)
			{
				GameObject newShimmer = Instantiate(shimmer);
				newShimmer.transform.position = waypoint.transform.position;
				fourxFourLimiter++;
				upperbound4++;
			}
		}

		foreach(GameObject waypoint in sixxSixArray)
		{
			int tester = Random.Range(0, upperbound6);
			if (tester == 2 && sixxSixLimiter < 3)
			{
				GameObject newShimmer = Instantiate(shimmer);
				newShimmer.transform.position = waypoint.transform.position;
				sixxSixLimiter++;
				upperbound6++;
			}
		}

		foreach(GameObject waypoint in eightxEightArray)
		{
			int tester = Random.Range(0, upperbound8);
			if (tester == 2 && eightxEightLimiter < 4)
			{
				GameObject newShimmer = Instantiate(shimmer);
				newShimmer.transform.position = waypoint.transform.position;
				eightxEightLimiter++;
				upperbound8++;
			}
		}
	}

	void PlaceEnemies()
	{
		int fourxFourLimiter = 0;
		int sixxSixLimiter = 0;
		int eightxEightLimiter = 0;
		int counter = 0;
		int upperbound4 = 3;
		int upperbound6 = 3;
		int upperbound8 = 8;

		foreach(GameObject waypoint in fourxFourArray)
		{
			counter++;


		
			int tester = Random.Range(0, upperbound4);
			if (tester == 2 && fourxFourLimiter < 1 && counter > 8)
			{
				GameObject newEnemy = Instantiate(enemy);
				newEnemy.transform.position = waypoint.transform.position;
				fourxFourLimiter++;
				upperbound4++;
			}

		}

		foreach(GameObject waypoint in sixxSixArray)
		{
			counter++;


			int tester = Random.Range(0, upperbound6);
			if (tester == 2 && sixxSixLimiter < 2 && counter > 12)
			{
				GameObject newEnemy = Instantiate(enemy);
				newEnemy.transform.position = waypoint.transform.position;
				sixxSixLimiter++;
				upperbound6++;
			}

		}
		foreach(GameObject waypoint in eightxEightArray)
		{
			counter++;


			int tester = Random.Range(0, upperbound8);
			if (tester == 2 && eightxEightLimiter < 3 && counter > 16)
			{
				GameObject newEnemy = Instantiate(enemy);
				newEnemy.transform.position = waypoint.transform.position;
				eightxEightLimiter++;
				upperbound8++;
			}

		}
	}

    private static double ComplexProbabillity(double at, double desired, double spread = 1)
        => Math.Pow(Math.Pow(10, Math.Log10(desired) / (at * spread)), spread);

    private static void FloorEach(GameObject[,] floor, Func<int, int, bool> inner, Func<int, bool> outer,
        Action<int, int> action)
    {
        var count = 1;
        do
        {
            int x;
            int z;
            do
            {
                x = Random.Range(0, floor.GetLength(0));
                z = Random.Range(0, floor.GetLength(1));
            } while (inner(x, z));
            action(x, z);
        } while (outer(count++));
    }

	//A 6x6 grid maker.
	void SixxSix()
	{
		bool hasMadePit = false;

		for (int x = 0; x < 7; x++)
		{
			for (int z = 0; z < 7; z++)
			{
				GameObject newPoint = Instantiate(empty);
				empty.transform.position = new Vector3 (x, -4, z);
				GameObject newTile = Instantiate(tileCube);
				newTile.transform.position = new Vector3(x, -5, z);

				sixxSixArray[x,z] = newPoint;

				if (hasMadePit == false)
				{
					int tester = Random.Range (0,10);
					if (tester == 5){
						Destroy(newTile);
						hasMadePit = true;
					}
				}
				if (hasMadePit == false && x == 4 && z == 4) {
					Destroy(newTile);
					hasMadePit = true;
				}
			}
		}
	}

	//An 8x8 grid maker.
	void EightxEight()
	{
		bool hasMadePit = false;

		for (int x = 0; x < 9; x++)
		{
			for (int z = 0; z < 9; z++)
			{
				GameObject newPoint = Instantiate(empty);
				empty.transform.position = new Vector3 (x, -9, z);
				GameObject newTile = Instantiate(tileCube);
				newTile.transform.position = new Vector3(x, -10, z);

				eightxEightArray[x,z] = newPoint;

				if (hasMadePit == false)
				{
					int tester = Random.Range (0,10);
					if (tester == 5){
						Destroy(newTile);
						hasMadePit = true;
					}
				}
				if (hasMadePit == false && x == 4 && z == 4) {
					Destroy(newTile);
					hasMadePit = true;
				}
			}
		}
	}
}