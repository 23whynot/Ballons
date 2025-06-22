using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.GamePlay.Audio.ScrObject
{
    [CreateAssetMenu(menuName = "Game/Audio List")]
    public class AudioList : ScriptableObject
    {
        public List<AudioClip> Audios = new List<AudioClip>();
    }
}