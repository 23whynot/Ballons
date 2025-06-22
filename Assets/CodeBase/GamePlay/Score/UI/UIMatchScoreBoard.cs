using System;
using TMPro;
using UnityEngine;
using Zenject;
using DG.Tweening;

namespace CodeBase.GamePlay.Score.UI
{
    public class UIMatchScoreBoard : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;
        private IScoreController _scoreController;
        private int _currentScore = 0;
        private Tween _scoreTween;

        [Inject]
        public void Construct(IScoreController scoreController) =>
            _scoreController = scoreController;

        private void OnEnable()
        {
            _currentScore = 0;
            scoreText.text = "0";
        }

        private void Awake() =>
            _scoreController.OnScoreChanged += AnimateScore;

        private void AnimateScore(int newScore)
        {
            _scoreTween?.Kill();
            
            _scoreTween = DOTween.To(
                () => _currentScore,
                value =>
                {
                    _currentScore = value;
                    scoreText.text = value.ToString();
                },
                newScore,
                0.5f
            ).SetEase(Ease.OutQuad);
            
            scoreText.transform.DOKill();
            scoreText.transform.DOPunchScale(Vector3.one * 0.2f, 0.3f, 5, 0.5f);
        }

        private void OnDestroy()
        {
            _scoreController.OnScoreChanged -= AnimateScore;
            _scoreTween?.Kill();
        }
    }
}