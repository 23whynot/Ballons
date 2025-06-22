using System;

namespace CodeBase.GamePlay.Score
{
    public interface IScoreController
    {
        public event Action<int> OnScoreChanged;
        
        void SetScore(int score);

        int GetScore();

        void ClearScore();
    }
}