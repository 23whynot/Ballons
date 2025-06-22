using System;
using CodeBase.GamePlay.Currency;
using CodeBase.GamePlay.LeaderboardWindow;
using CodeBase.GamePlay.Level.Holder;
using CodeBase.GamePlay.UI.Windows;
using CodeBase.Infrastructure.UI.Window;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.GamePlay.Window.LeaderBoard.Elements
{
    public class ClaimButton : MonoBehaviour
    {
        [SerializeField] private Button claimButton;
        private IWindowManager _windowManager;
        private ICurrencyController _currencyController;
        private ILevelHolder _levelHolder;

        [Inject]
        public void Construct(IWindowManager windowManager, 
            ICurrencyController currencyController,
            ILevelHolder levelHolder)
        {
            _levelHolder = levelHolder;
            _windowManager = windowManager;
            _currencyController = currencyController;
        }

        private void Awake() => 
            claimButton.onClick.AddListener(Claim);

        private void Claim()
        {
            _windowManager.OpenInitializableWindowOnHudAsync(WindowAssetsPath.PrizeWindow,
                new Prize(PrizeType.Gem, _levelHolder.GetConfig().Prize));
            _currencyController.Increase(_levelHolder.GetConfig().Prize);
        }

        private void OnDestroy() => 
            claimButton.onClick.RemoveListener(Claim);
    }
}