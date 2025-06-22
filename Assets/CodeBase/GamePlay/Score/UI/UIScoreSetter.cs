using TMPro;
using UnityEngine;
using Zenject;

namespace CodeBase.GamePlay.Score.UI
{
    public class UIScoreSetter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI score;
        
        private IScoreController _scoreController;

        [Inject]
        public void Construct(IScoreController scoreController) => 
            _scoreController = scoreController;

        private void OnEnable() => 
            score.text = _scoreController.GetScore().ToString();
    }
}