using System;
using TMPro;
using UnityEngine;
using Zenject;
using DG.Tweening;

namespace CodeBase.GamePlay.Currency.UI
{
    public class CurrencySetter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI count;
        
        private ICurrencyController _currencyController;
        private int _currentAmount;
        private Tweener _tweener;

        [Inject]
        public void Construct(ICurrencyController currencyController) => 
            _currencyController = currencyController;

        private void Awake() => 
            _currencyController.CurrentAmountChanged += ChangeCount;

        private void OnEnable()
        {
            _currentAmount = _currencyController.CurrentAmount;
            count.text = _currentAmount.ToString();
        }

        private void ChangeCount(int newAmount)
        {
            _tweener?.Kill();

            int startValue = _currentAmount;
            int endValue = newAmount;
            float duration = 0.5f;

            _tweener = DOVirtual.Int(startValue, endValue, duration, value =>
            {
                count.text = value.ToString();
            }).SetEase(Ease.OutQuad);

            AnimatePunch();
            _currentAmount = newAmount;
        }

        private void AnimatePunch()
        {
            count.rectTransform.DOKill();
            count.rectTransform.DOPunchScale(Vector3.one * 0.2f, 0.3f, vibrato: 2, elasticity: 0.4f);
        }

        private void OnDestroy() => 
            _currencyController.CurrentAmountChanged -= ChangeCount;
    }
}