using UnityEngine;

namespace CodeBase.GamePlay.Audio.Player
{
    public interface IAudioPlayer
    {
        void Play(AudioType audioType ,AudioClip audioClip, bool loop = false);
        void SetVolume(AudioType audioType, float volume);
    }
}