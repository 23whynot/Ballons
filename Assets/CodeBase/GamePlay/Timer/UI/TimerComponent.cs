using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using TMPro;

namespace CodeBase.GamePlay.Timer.UI
{
    public class TimerComponent : MonoBehaviour
    {
        [SerializeField] private Image fillImage;
        [SerializeField] private TextMeshProUGUI timerText;

        private ITimerOnLevelController _timerController;
        private int _duration;
        private float _timeLeft;
        private Coroutine _timerCoroutine;

        [Inject]
        public void Construct(ITimerOnLevelController timerController) =>
            _timerController = timerController;

        private void Awake() =>
            _timerController.OnStartTimer += StartTimer;

        private void StartTimer(int duration)
        {
            if (_timerCoroutine != null)
                StopCoroutine(_timerCoroutine);

            _duration = duration;
            _timeLeft = duration;
            _timerCoroutine = StartCoroutine(TimerRoutine());
        }

        private IEnumerator TimerRoutine()
        {
            while (_timeLeft > 0)
            {
                _timeLeft -= Time.deltaTime;
                UpdateUI(_timeLeft);
                yield return null;
            }

            TimeEnd();
        }

        private void UpdateUI(float timeLeft)
        {
            timeLeft = Mathf.Clamp(timeLeft, 0, _duration);

            if (fillImage != null)
                fillImage.fillAmount = timeLeft / _duration;

            if (timerText != null)
            {
                int seconds = Mathf.CeilToInt(timeLeft);
                timerText.text = seconds + " s.";
            }
        }

        private void TimeEnd()
        {
            timerText.text = "0";
            fillImage.fillAmount = 0f;

            _timerController.TimeEnd();
        }

        private void OnDestroy()
        {
            if (_timerCoroutine != null)
                StopCoroutine(_timerCoroutine);

            _timerController.OnStartTimer -= StartTimer;
        }
    }
}
