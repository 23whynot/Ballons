using System.Collections.Generic;
using System.Linq;
using CodeBase.GamePlay.Audio.Player;
using CodeBase.GamePlay.Audio.ScrObject;
using CodeBase.Infrastructure.AssetManagement;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CodeBase.GamePlay.Audio
{
    public class AudioService : IAudioService
    {
        private IAssetProvider _assetProvider;
        private IAudioPlayer _audioPlayer;
        private List<AudioClip> _audios = new();
        
        private const string MusicVolumeKey = "AUDIO_VOLUME_Music";
        private const string SoundVolumeKey = "AUDIO_VOLUME_Sound";

        [Inject]
        public void Construct(IAssetProvider assetProvider) => 
            _assetProvider = assetProvider;

        public async UniTask InitializeAsync()
        {
            var audioList = await _assetProvider.Load<AudioList>(AssetPath.AudioList);
            _audios = audioList.Audios;
            
            float musicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 1f);
            float soundVolume = PlayerPrefs.GetFloat(SoundVolumeKey, 1f);

            _audioPlayer.SetVolume(AudioType.Music, musicVolume);
            _audioPlayer.SetVolume(AudioType.Sound, soundVolume);
            
            _audioPlayer.Play(AudioType.Music, GetSoundByID(AudioAssetPath.MainTheme), true);
        }

        public void SetAudioPlayer(IAudioPlayer audioPlayer) => 
            _audioPlayer = audioPlayer;

        public void SetVolume(AudioType type, float value)
        {
            _audioPlayer?.SetVolume(type, value);

            if (type == AudioType.Music)
                PlayerPrefs.SetFloat(MusicVolumeKey, value);
            else if (type == AudioType.Sound)
                PlayerPrefs.SetFloat(SoundVolumeKey, value);

            PlayerPrefs.Save();
        }

        public void Play(AudioType type, string soundID, bool loop = false) =>
            _audioPlayer.Play(type, GetSoundByID(soundID), loop);

        public float GetVolume(AudioType type)
        {
            return type switch
            {
                AudioType.Music => PlayerPrefs.GetFloat("AUDIO_VOLUME_Music", 1f),
                AudioType.Sound => PlayerPrefs.GetFloat("AUDIO_VOLUME_Sound", 1f),
                _ => 1f
            };
        }

        private AudioClip GetSoundByID(string soundID) =>
            _audios.FirstOrDefault(clip => clip.name == soundID);
    }
}
