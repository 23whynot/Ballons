using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.Infrastructure.UI.LoadingCurtain
{
    public class LoadingCurtain : MonoBehaviour, ILoadingCurtain
    {
        [SerializeField] private CanvasGroup canvas;
        [SerializeField] private Image progressBar;
        private float _fadeDuration = 0.5f;
        private float _fillDuration = 0.5f;

        private Tween _fillTween;
        private Tween _fadeTween;

        public async UniTask Show()
        {
            canvas.alpha = 0;
            progressBar.fillAmount = 0;
            gameObject.SetActive(true);

            await canvas.DOFade(1, _fadeDuration)
                .SetEase(Ease.InOutQuad)
                .AsyncWaitForCompletion();
        }

        public async UniTask HideAsync()
        {
            await DoFillProgress(100); // Дожидаемся заполнения
            await DoFadeOut(); // Дожидаемся скрытия
        }

        public async UniTask UpdateProgress(float progress) =>
            await DoFillProgress(progress);

        private async UniTask DoFillProgress(float targetProgress)
        {
            _fillTween?.Kill();
            _fillTween = progressBar
                .DOFillAmount(targetProgress / 100f, _fillDuration)
                .SetEase(Ease.Linear);

            await _fillTween.AsyncWaitForCompletion(); // Ожидаем завершения
        }

        private async UniTask DoFadeOut()
        {
            _fadeTween?.Kill();
            _fadeTween = canvas
                .DOFade(0, _fadeDuration)
                .SetEase(Ease.Linear)
                .OnComplete(() => gameObject.SetActive(false));

            await _fadeTween.AsyncWaitForCompletion(); // Ожидаем завершения
        }

        public class Factory : PlaceholderFactory<string, UniTask<LoadingCurtain>>
        {
        }
    }
}