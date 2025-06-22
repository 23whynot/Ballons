using CodeBase.GamePlay.Audio;
using DG.Tweening;
using UnityEngine;
using Zenject;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine.UI;
using AudioType = CodeBase.GamePlay.Audio.AudioType;

namespace CodeBase.GamePlay.Ballon
{
    public class BallonSoundAndAnimationComponent : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private float explosionDuration = 0.3f;
        [SerializeField] private float maxScale = 1.5f;

        private IAudioService _audioService;

        [Inject]
        public void Construct(IAudioService audioService) => 
            _audioService = audioService;

        public void Initialize(BalloonConfig config) => 
            scoreText.text = "+" + config.score;

        private void OnEnable()
        {
            transform.localScale = Vector3.one;
            
            scoreText.gameObject.SetActive(false);
            scoreText.rectTransform.localScale = Vector3.one;
            
            var color = scoreText.color;
            color.a = 1f;
            scoreText.color = color;
            
            image.enabled = true;
            image.rectTransform.localScale = Vector3.one;
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
        }

        private void OnDisable()
        {
            scoreText.rectTransform.localScale = Vector3.one;
            scoreText.gameObject.SetActive(false);
        }

        public async UniTask PlayAnimationAsync()
        {
            _audioService.Play(AudioType.Sound, AudioAssetPath.Explosion, false);
            
            await image.rectTransform.DOScale(maxScale, explosionDuration / 2f)
                .SetEase(Ease.OutQuad)
                .ToUniTask();

            await image.rectTransform.DOScale(0f, explosionDuration / 2f)
                .SetEase(Ease.InQuad)
                .ToUniTask();
            
            image.enabled = false;
            
            scoreText.gameObject.SetActive(true);
            
            var color = scoreText.color;
            color.a = 0f;
            scoreText.color = color;
            
            scoreText.rectTransform.localScale = Vector3.zero;
            
            await UniTask.WhenAll(
                scoreText.DOColor(new Color(color.r, color.g, color.b, 1f), explosionDuration).ToUniTask(),
                scoreText.rectTransform.DOScale(1f, explosionDuration).SetEase(Ease.OutBack).ToUniTask()
            );

            await UniTask.WaitForSeconds(1f);
        }
    }
}
