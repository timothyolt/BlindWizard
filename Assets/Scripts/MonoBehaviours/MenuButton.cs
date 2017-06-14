using UnityEngine;
using UnityEngine.SceneManagement;

namespace MonoBehaviours
{
	[RequireComponent(typeof(Renderer))]
	public class MenuButton : MonoBehaviour
	{

		[SerializeField] private Color _inactiveColor, _activeColor;
		private Renderer _renderer;
		public GameObject _scorebox;
		private void Start()
		{
			// didnt work _scorebox = GameObject.FindGameObjectWithTag("scorebox");
			_renderer = GetComponent<Renderer>();
			_renderer.material.color = _inactiveColor;
		}

		public void SetGazedAt(bool gazed)
		{
			if (gazed)
			{
				transform.localPosition += new Vector3(0, 0.1f, 0);
				_renderer.material.color = _activeColor;
			}
			else
			{
				transform.localPosition -= new Vector3(0, 0.1f, 0);
				_renderer.material.color = _inactiveColor;
			}
		}

		public void ShowMenu()
		{
			_scorebox.SetActive(true);
		}
		public void Resume()
		{
			_scorebox.SetActive(false);
		}
		public void Restart()
		{
			SceneManager.LoadScene(0);
		}
	}
}
