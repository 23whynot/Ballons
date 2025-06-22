using CodeBase.Infrastructure.UI.Window;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace CodeBase.Infrastructure.UI.GUI
{
    public class BaseGuiAnimationAndSoundWindowComponent : MonoBehaviour, IWindow
    {
        [SerializeField] private CanvasGroup canvas;

        Tween _tween;

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
            //SoundMaster.Instance.SoundPlayOpenWindow(0.1f);  
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