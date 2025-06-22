using System;
using Cysharp.Threading.Tasks;

namespace CodeBase.GamePlay.Currency
{
    public interface ICurrencyController
    {
        public event Action<int> CurrentAmountChanged;
        public int CurrentAmount { get; }

        UniTask InitializeAsync();

        public void Increase(int amount);
        public void Decrease(int amount);
    }
}