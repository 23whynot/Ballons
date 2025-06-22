using System;
using CodeBase.GamePlay.Audio.Player;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CodeBase.GamePlay.Audio
{
    public interface IAudioService
    {
        UniTask InitializeAsync();
        void SetVolume(AudioType type, float value);
        void Play(AudioType type, string soundID, bool loop = false);
        public float GetVolume(AudioType type);
        void SetAudioPlayer(IAudioPlayer audioPlayer);
    }
}