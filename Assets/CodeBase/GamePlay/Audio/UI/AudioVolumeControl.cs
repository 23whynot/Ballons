using UnityEngine;
using UnityEngine.UI;
using Zenject;
using DG.Tweening;
using UnityEngine.EventSystems;

namespace CodeBase.GamePlay.Audio.UI
{
    public class AudioVolumeControl : MonoBehaviour
    {
        [SerializeField] private AudioType audioType;
        [SerializeField] private Image fillImage;
        [SerializeField] private Button increaseButton;
        [SerializeField] private Button decreaseButton;
        [SerializeField] private Button saveButton;

        private IAudioService _audioService;
        private float _durationToFull = 2f;
        private Tweener _tweener;
        private float _initialVolume;
        private bool _isSaved;

        [Inject]
        public void Construct(IAudioService audioService) =>
            _audioService = audioService;

        private void OnEnable()
        {
            if (_audioService == null || fillImage == null)
                return;

            _initialVolume = _audioService.GetVolume(audioType);
            fillImage.fillAmount = _initialVolume;
            _isSaved = false;

            AddButtonEvents(increaseButton, IncreaseVolumeStart, StopTween);
            AddButtonEvents(decreaseButton, DecreaseVolumeStart, StopTween);

            if (saveButton != null)
                saveButton.onClick.AddListener(SaveVolume);
        }

        private void OnDisable()
        {
            StopTween();

            if (!_isSaved)
            {
                _audioService.SetVolume(audioType, _initialVolume);
                fillImage.fillAmount = _initialVolume;
            }

            if (saveButton != null)
                saveButton.onClick.RemoveListener(SaveVolume);
        }

        private void AddButtonEvents(Button button, System.Action onPress, System.Action onRelease)
        {
            if (button == null)
                return;

            var trigger = button.gameObject.GetComponent<EventTrigger>();
            if (trigger == null)
                trigger = button.gameObject.AddComponent<EventTrigger>();

            trigger.triggers.Clear();

            var pointerDown = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
            pointerDown.callback.AddListener(_ => onPress());
            trigger.triggers.Add(pointerDown);

            var pointerUp = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
            pointerUp.callback.AddListener(_ => onRelease());
            trigger.triggers.Add(pointerUp);

            var pointerExit = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
            pointerExit.callback.AddListener(_ => onRelease());
            trigger.triggers.Add(pointerExit);
        }

        private void IncreaseVolumeStart() =>
            StartTween(1f);

        private void DecreaseVolumeStart() =>
            StartTween(0f);

        private void StartTween(float targetFill)
        {
            _tweener?.Kill();

            float currentFill = fillImage.fillAmount;
            float distance = Mathf.Abs(targetFill - currentFill);
            float duration = distance * _durationToFull;

            _tweener = fillImage.DOFillAmount(targetFill, duration)
                .SetEase(Ease.Linear)
                .OnUpdate(() =>
                {
                    _audioService.SetVolume(audioType, fillImage.fillAmount);
                });
        }

        private void StopTween() =>
            _tweener?.Kill();

        private void SaveVolume()
        {
            _initialVolume = fillImage.fillAmount;
            _isSaved = true;
        }
    }
}
