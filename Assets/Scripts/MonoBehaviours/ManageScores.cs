using BlindWizard.Data;
using UnityEngine;
using UnityEngine.UI;

namespace BlindWizard.MonoBehaviours
{
    public class ManageScores : MonoBehaviour
    {
        [SerializeField]
        private Text[] _kills, _turns, _shimmers, _blocks, _floors;

        private bool _showScores;

        public void SetShowScores(bool showScores)
        {
            _showScores = showScores;
            gameObject.SetActive(_showScores);
        }

        private void Update()
        {
            if (!_showScores)
                return;
            foreach (var kills in _kills)
                kills.text = "Kills: " + Score.Kills;
            foreach (var turns in _turns)
                turns.text = "Turns: " + Score.Turns;
            foreach (var blocks in _blocks)
                blocks.text = "Blocks: " + Score.Blocks;
            foreach (var floors in _floors)
                floors.text = "floors: " + Score.Floors;
            foreach (var shimmers in _shimmers)
                shimmers.text = "shimmers: " + Score.Shimmers;
        }
    }
}