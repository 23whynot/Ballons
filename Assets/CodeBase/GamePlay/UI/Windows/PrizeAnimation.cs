using CodeBase.GamePlay.LeaderboardWindow;
using CodeBase.Infrastructure.UI.Window;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.GamePlay.UI.Windows
{
    public class PrizeAnimation : MonoBehaviour, IInitializableWindow<Prize>
    {
        [SerializeField] private Transform gemTarget;
        [SerializeField] private Image[] prizes;
        [SerializeField] private Sprite gemSprite;
        [SerializeField] private TextMeshProUGUI prizeCountText;
        [SerializeField] private CanvasGroup canvas;

        private Tween _tween;
        private Prize _currentPrize;
        private Vector3[] _initialPositions;
        private bool _isPositionSaved;
        private bool _isCreated;
        private IWindowManager _windowManager;

        [Inject]
        public void Construct(IWindowManager windowManager) =>
            _windowManager = windowManager;

        private void OnEnable()
        {
            if (_isCreated)
            {
                if (_isPositionSaved)
                    ResetPositions();
                else
                    SaveInitialPositions();
            }

            _isCreated = true;
        }

        public void Initialize(Prize prize) =>
            _currentPrize = prize;

        public GameObject GetGameObject() =>
            gameObject;

        public UniTask BeforeOpenAsync() => UniTask.CompletedTask;
        public UniTask AfterOpenAsync() => UniTask.CompletedTask;
        public UniTask OpenAsync() => UniTask.CompletedTask;
        public UniTask BeforeCloseAsync() => UniTask.CompletedTask;
        public UniTask AfterCloseAsync() => UniTask.CompletedTask;

        public void BeforeOpen()
        {
            canvas.alpha = 0f;
            gameObject.SetActive(true);
        }

        public void Open()
        {
            _tween?.Kill();
            _tween = canvas.DOFade(1f, 0.5f).OnComplete(() => _tween.Kill());
        }

        public void AfterOpen() =>
            StartAnimation();

        public void BeforeClose() { }

        public async UniTask CloseAsync()
        {
            _tween?.Kill();
            _tween = canvas.DOFade(0f, 0.5f).OnComplete(() => _tween.Kill());
            gameObject.SetActive(false);
        }

        public void AfterClose() =>
            _windowManager.ProcessQueueOnGui();

        private void StartAnimation()
        {
            SetPrizeSprite(gemSprite);
            ActivateAllImages();
            AnimateImages(gemTarget);
            prizeCountText.text = GetPrizeCountText();
        }

        private string GetPrizeCountText() =>
            _currentPrize.PrizeType switch
            {
                PrizeType.Cash => "$" + _currentPrize.Count,
                PrizeType.Gem => _currentPrize.Count.ToString(),
                _ => string.Empty
            };

        private void SaveInitialPositions()
        {
            _initialPositions = new Vector3[prizes.Length];
            for (int i = 0; i < prizes.Length; i++)
                _initialPositions[i] = prizes[i].transform.position;

            _isPositionSaved = true;
        }

        private void ResetPositions()
        {
            for (int i = 0; i < prizes.Length; i++)
                prizes[i].transform.position = _initialPositions[i];
        }

        private void ActivateAllImages()
        {
            foreach (var prize in prizes)
                prize.gameObject.SetActive(true);
        }

        private void AnimateImages(Transform target)
        {
            DOTween.Kill(this);

            float moveDuration = 0.35f;
            float delayBetween = 0.05f;

            for (int i = 0; i < prizes.Length; i++)
            {
                var prize = prizes[i];
                var prizeTransform = prize.transform;

                prizeTransform.position = _initialPositions[i];
                prizeTransform.localScale = Vector3.one;

                var startPos = _initialPositions[i];
                var endPos = target.position;
                var control1 = startPos + Vector3.up * 150f + Vector3.left * 100f;
                var control2 = endPos + Vector3.up * 100f + Vector3.right * 100f;

                var seq = DOTween.Sequence();

                seq.Append(prizeTransform
                    .DOShakePosition(0.15f, new Vector3(10f, 10f, 0f), 10)
                    .SetEase(Ease.OutQuad));

                seq.Append(DOTween.To(
                        t =>
                        {
                            prizeTransform.position = CalculateCubicBezierPoint(t, startPos, control1, control2, endPos);
                            var scale = Mathf.Lerp(1f, 0.4f, t);
                            prizeTransform.localScale = Vector3.one * scale;
                        },
                        0f, 1f, moveDuration)
                    .SetEase(Ease.InOutQuad)
                    .OnComplete(() => DeactivateAndReset(prize.gameObject, prizeTransform))
                    .SetDelay(i * delayBetween));
            }

            float totalDelay = (prizes.Length - 1) * delayBetween + moveDuration;
            DOVirtual.DelayedCall(totalDelay + 0.1f,
                () => _windowManager.CloseWindowByIdOnHud(WindowAssetsPath.PrizeWindow)).SetId(this);
        }

        private Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;
            float uuu = uu * u;
            float ttt = tt * t;

            return uuu * p0 + 3 * uu * t * p1 + 3 * u * tt * p2 + ttt * p3;
        }

        private void DeactivateAndReset(GameObject obj, Transform baseTransform)
        {
            obj.SetActive(false);
            obj.transform.position = baseTransform.position;
        }

        private void SetPrizeSprite(Sprite sprite)
        {
            foreach (var prize in prizes)
                prize.sprite = sprite;
        }
    }
}
