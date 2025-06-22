using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.GamePlay.Audio.UI
{
    public class UIButtonClick : MonoBehaviour
    {
        [SerializeField] private Button button;
        private IAudioService _audioService;

        [Inject]
        public void Construct(IAudioService audioService) => 
            _audioService = audioService;

        private void Awake() => 
            button.onClick.AddListener(PlayAudio);

        private void PlayAudio() => 
            _audioService.Play(AudioType.Sound, AudioAssetPath.Button);

        private void OnDestroy() => 
            button.onClick.RemoveListener(PlayAudio);
    }
}