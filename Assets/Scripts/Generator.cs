using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Generator : MonoBehaviour {

	//Prefabs
    [SerializeField]
	private GameObject _floor, _wall, _shimmer, _enemy;

    public List<WizardLevel> FinalLevels { get; } = new List<WizardLevel>();

    private void Start ()
	{
	    for (var i = 1; i < 25; i++)
	        FinalLevels.Add(new WizardLevel(i, _floor, _wall, _shimmer, _enemy));
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        levels.Add(new WizardLevel(1, tileCube, wallHorizontal, shimmer, enemy));
        levels.Add(new WizardLevel(2, tileCube, wallHorizontal, shimmer, enemy));
	    levels.Add(new WizardLevel(3, tileCube, wallHorizontal, shimmer, enemy));
        levels.Add(new WizardLevel(4, tileCube, wallHorizontal, shimmer, enemy));
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
        if (Input.GetKeyDown(KeyCode.Space))
            SceneManager.LoadScene(0);
    }
}
    [SerializeField]
	private GameObject _floor, _wall, _shimmer, _enemy;
    private void Start ()
    private void Update()