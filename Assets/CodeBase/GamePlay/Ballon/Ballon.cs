using System;
using CodeBase.GamePlay.Score;
using CodeBase.Infrastructure.ObjPool;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.GamePlay.Ballon
{
    [RequireComponent(typeof(BallonSoundAndAnimationComponent))]
    public class Ballon : MonoBehaviour, IPoolableObject
    {
        [SerializeField] private Button button;
        [SerializeField] private Image image;
        [SerializeField] private BallonSoundAndAnimationComponent soundAndAnimation;
        [SerializeField] private float swayAmount = 20f;
        [SerializeField] private float swayDuration = 1.5f;

        private float _flyDuration = 2.5f;
        private float _flyHeight = 3000f;
        private Tween _flyTween;
        private Tween _swayTween;

        [Inject]
        public void Construct(IScoreController scoreController)
        {
            _scoreController = scoreController;
        }
        
        private bool _isExploding;
        private IScoreController _scoreController;
        private Vector3 _startLocalPosition;
        private BalloonConfig _config;

        public bool IsActive { get; private set; }

        private void Awake() => 
            button.onClick.AddListener(ExploadeCall);

        private void ExploadeCall() => 
            Exploade().Forget();

        private void OnEnable() => 
            FlyUp();

        private void OnDestroy() => 
            button.onClick.RemoveListener(ExploadeCall);

        public void Initialize(BalloonConfig getRandomBoughtBalloonConfig)
        {
            _config = getRandomBoughtBalloonConfig;
            image.sprite = _config.Icon; 
            
            soundAndAnimation.Initialize(_config);
        }

        private void OnDisable()
        {
            _flyTween?.Kill();
            _swayTween?.Kill();
        }

        public void Activate()
        {
            _startLocalPosition = transform.localPosition = Vector3.zero; 
            gameObject.SetActive(true);
            IsActive = true;
            _isExploding = false;
        }

        private void FlyUp()
        {
            transform.localPosition = _startLocalPosition;

            _flyTween = transform.DOLocalMoveY(_startLocalPosition.y + _flyHeight, _flyDuration)
                .SetEase(Ease.Linear)
                .OnComplete(Deactivate);

            _swayTween = transform.DOLocalMoveX(_startLocalPosition.x + swayAmount, swayDuration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }


        public void Deactivate()
        {
            gameObject.SetActive(false);
            IsActive = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_isExploding)
                return;

            Exploade().Forget();
        }

        private async UniTask Exploade()
        {
            _isExploding = true;
            
            _scoreController.SetScore(_scoreController.GetScore() + _config.score);
            
            KillTweens();
            await soundAndAnimation.PlayAnimationAsync();
            Deactivate();
        }

        private void KillTweens()
        {
            _flyTween?.Kill();
            _swayTween?.Kill();
        }
    }
}
