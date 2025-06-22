using CodeBase.GamePlay.Shop.Elements;
using CodeBase.GamePlay.UI.Windows;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.GamePlay.Window.Shop
{
    public class BuyingWindow: MonoBehaviour, IInitializableWindow<Sprite>
    {
        [SerializeField] private CanvasGroup canvas;
        [SerializeField] private Image ballImage;
        [SerializeField] private BuyingButton buyingButton;

        Tween _tween;

        public void Initialize(Sprite parametr)
        {
            ballImage.sprite = parametr;
            buyingButton.Initialize(parametr.name);
        }

        public UniTask AfterCloseAsync()
        {
            return UniTask.CompletedTask;
        }

        public void BeforeOpen()
        {
            canvas.alpha = 0f;
            gameObject.SetActive(true);
        }

        public UniTask BeforeOpenAsync()
        {
            return UniTask.CompletedTask;
        }

        public UniTask AfterOpenAsync()
        {
            return UniTask.CompletedTask;
        }

        public void Open()
        {
            _tween?.Kill();
            _tween = canvas.DOFade(1f, 0.5f).OnComplete(() => _tween.Kill());
        }

        public UniTask OpenAsync()
        {
            return UniTask.CompletedTask;
        }

        public void AfterOpen()
        {
            
        }

        public GameObject GetGameObject() =>
            gameObject;

        public void BeforeClose()
        {
        }

        public UniTask BeforeCloseAsync()
        {
            return UniTask.CompletedTask;
        }

        public async UniTask CloseAsync()
        {
            _tween?.Kill();
            _tween = canvas.DOFade(0f, 0.5f).OnComplete(() => _tween.Kill());
        }

        public void AfterClose()
        {
            // SoundMaster.Instance.SoundPlayCloseWindow(0.1f);
            gameObject.SetActive(false);
        }
    }
}