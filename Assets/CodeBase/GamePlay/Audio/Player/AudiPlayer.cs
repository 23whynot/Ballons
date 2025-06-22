using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CodeBase.GamePlay.Audio.Player
{
    public class AudioPlayer : MonoBehaviour, IAudioPlayer
    {
        private readonly Dictionary<AudioType, AudioSource> _sources = new();
        private IAudioService _audioService;

        [Inject]
        public void Construct(IAudioService audioService) => 
            _audioService = audioService;

        private void Awake() => 
            _audioService.SetAudioPlayer(this);

        public void Play(AudioType audioType, AudioClip audioClip, bool loop = false)
        {
            if (audioClip == null)
                return;

            AudioSource source = GetOrCreateSource(audioType);
            source.clip = audioClip;
            source.loop = loop;
            source.Play();
        }

        public void SetVolume(AudioType audioType, float volume)
        {
            AudioSource source = GetOrCreateSource(audioType);
            source.volume = Mathf.Clamp01(volume);
        }

        private AudioSource GetOrCreateSource(AudioType type)
        {
            if (_sources.TryGetValue(type, out var source))
                return source;

            source = gameObject.AddComponent<AudioSource>();
            _sources[type] = source;
            return source;
        }
    }
}