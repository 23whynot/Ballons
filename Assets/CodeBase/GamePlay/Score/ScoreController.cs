using System;

namespace CodeBase.GamePlay.Score
{
    public class ScoreController : IScoreController
    {
        private int _currentScore;

        public event Action<int> OnScoreChanged;

        public void SetScore(int score)
        {
            _currentScore = score;
            OnScoreChanged?.Invoke(_currentScore);
        }

        public int GetScore() =>
            _currentScore;

        public void ClearScore() =>
            _currentScore = 0;
    }
}