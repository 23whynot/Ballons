using System;
using CodeBase.Infrastructure.Services.SaveLoad;
using Cysharp.Threading.Tasks;
using Zenject;

namespace CodeBase.GamePlay.Currency
{
    public class CurrencyController : ICurrencyController
    {
        private ISaveLoadService _saveLoadService;
        public event Action<int> CurrentAmountChanged;
        public int CurrentAmount { get; private set; }

        [Inject]
        public void Construct(ISaveLoadService saveLoadService) =>
            _saveLoadService = saveLoadService;

        public async UniTask InitializeAsync()
        {
            if (_saveLoadService.GameData.IsFirstLaunch)
            {
                CurrentAmount = 1000;

                _saveLoadService.GameData.IsFirstLaunch = false;
                
                UpdateData();
            }
            else
            {
                CurrentAmount = _saveLoadService.GameData.GemsAmount;
            }
        }

        public void Increase(int amount)
        {
            CurrentAmount += amount;
            CurrentAmountChanged?.Invoke(CurrentAmount);
            
            UpdateData();
        }

        public void Decrease(int amount)
        {
            CurrentAmount -= amount;
            CurrentAmountChanged?.Invoke(CurrentAmount);
            
            UpdateData();
        }

        private void UpdateData()
        {
            _saveLoadService.GameData.GemsAmount = CurrentAmount;
            _saveLoadService.Update(_saveLoadService.GameData);
        }
    }
}