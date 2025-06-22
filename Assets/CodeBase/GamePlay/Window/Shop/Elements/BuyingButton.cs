using System;
using CodeBase.GamePlay.Ballon.Controller;
using CodeBase.GamePlay.Currency;
using CodeBase.Infrastructure.UI.Window;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.GamePlay.Shop.Elements
{
    public class BuyingButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private Image buttonImage;
        [SerializeField] private Sprite greenButton;
        [SerializeField] private Sprite greyButton;
        
        private IBuyingBalloonController _buyingBalloonController;
        private string _id;
        private ICurrencyController _currencyController;
        private IWindowManager _windowManager;

        [Inject]
        public void Construct(IBuyingBalloonController buyingBalloonController,
            ICurrencyController currencyController,
            IWindowManager windowManager)
        {
            _windowManager = windowManager;    
            _buyingBalloonController = buyingBalloonController;
            _currencyController = currencyController;
        }

        private void Awake() => 
            button.onClick.AddListener(Buy);

        public void Initialize(string id)
        {
            _id = id;

            if (_currencyController.CurrentAmount >= 1000)
            {
                button.interactable = true;
                buttonImage.sprite = greenButton;
            }
            else
            {
                button.interactable = false;
                buttonImage.sprite = greyButton;
            }
        }

        private void Buy()
        {
            _windowManager.CloseCurrentWindowAsyncOnGui();
            _currencyController.Decrease(1000);
            _buyingBalloonController.MarkBalloonBuying(_id);
        }

        private void OnDestroy() =>
            button.onClick.RemoveListener(Buy);
    }
}