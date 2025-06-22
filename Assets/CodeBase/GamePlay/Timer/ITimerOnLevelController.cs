using System;

namespace CodeBase.GamePlay.Timer
{
    public interface ITimerOnLevelController
    {
        public event Action<int> OnStartTimer;

        public void StartTimer(int timer);

        public void TimeEnd();
    }
}