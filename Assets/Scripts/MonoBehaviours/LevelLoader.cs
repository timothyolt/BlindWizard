using UnityEngine;
using UnityEngine.SceneManagement;

namespace Blindwizard.MonoBehaviours
{
	public class LevelLoader : MonoBehaviour {

		// Use this for initialization
		void Start () {
		
		}
	
		// Update is called once per frame
		void Update () {
		
		}

		public void StartGame()
		{
			SceneManager.LoadScene("Main");
		}

		public void QuitGame()
		{
			Application.Quit();
		}
	}
}
