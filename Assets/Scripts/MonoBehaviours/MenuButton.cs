using UnityEngine;
using UnityEngine.SceneManagement;

namespace BlindWizard.MonoBehaviours
{
	[RequireComponent(typeof(Renderer))]
	public class MenuButton : MonoBehaviour
	{

		[SerializeField] private Color _inactiveColor, _activeColor;
		private Renderer _renderer;
		[SerializeField] private ManageScores _score;

		private void Start()
		{
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
			_score.ToggleShowScores();
		}
	}
}
