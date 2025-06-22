using System;
using CodeBase.GamePlay.Ballon.Controller;
using CodeBase.Infrastructure.UI.Window;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.GamePlay.Window.Shop.Elements
{
    public class ShopPlate : MonoBehaviour
    {
        [SerializeField] private Image ballImage;
        [SerializeField] public TextMeshProUGUI costText;
        [SerializeField] private Button buyButton;
        
        private bool _isInitialized = false;
        
        private IBuyingBalloonController _buyingBalloonController;
        private IWindowManager _windowManager;

        [Inject]
        public void Construct(IBuyingBalloonController buyingBalloonController, 
            IWindowManager  windowManager)
        {
            _windowManager = windowManager; 
            _buyingBalloonController = buyingBalloonController;
        }

        private void Awake() => 
            buyButton.onClick.AddListener(OpenWindow);

        private void OpenWindow() => 
            _windowManager.OpenInitializableWindowOnGuiAsync(WindowAssetsPath.BuyingWindow, ballImage.sprite);

        private void OnEnable()
        {
            if(!_isInitialized)
                return;
            
            if (_buyingBalloonController.IsBuyingBalloon(ballImage.sprite.name))
            {
                costText.text = "bought";
                buyButton.interactable = false;
            }
            else
            {
                costText.text = "1000";
                buyButton.interactable = true;
            }
        }

        public void Initialize(Sprite sprite)
        {
            ballImage.sprite = sprite;

            _isInitialized = true;
        }
        

        public void Activate() =>
            gameObject.SetActive(true);

        public void Deactivate() =>
            gameObject.SetActive(false);

        private void OnDestroy() => 
            buyButton.onClick.RemoveListener(OpenWindow);
    }
}