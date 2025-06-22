using System;
using CodeBase.Infrastructure.UI.Window;
using UnityEngine;
using Zenject;

namespace CodeBase.GamePlay.Timer
{
    public class TimerOnLevelController : ITimerOnLevelController
    {
        private IWindowManager _windowManager;
        public event Action<int> OnStartTimer;

        [Inject]
        public void Construct(IWindowManager windowManager) => 
            _windowManager = windowManager;

        public void StartTimer(int timer) => 
            OnStartTimer?.Invoke(timer);

        public void TimeEnd() => 
            _windowManager.OpenWindowAsyncOnGui(WindowAssetsPath.ScoreWindow);
    }
}