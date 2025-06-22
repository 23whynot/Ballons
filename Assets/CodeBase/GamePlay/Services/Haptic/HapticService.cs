using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CodeBase.GamePlay.Services.Haptic
{
    public class HapticService : IHapticService
    {
        private const string HAPTIC_PREF_KEY = "HapticEnabled";
        private bool _isHapticEnabled = true;

        public bool IsHapticEnabled => _isHapticEnabled;

        public async UniTask InitializeAsync() => 
            _isHapticEnabled = PlayerPrefs.GetInt(HAPTIC_PREF_KEY, 0) == 1;

        public void SetHapticEnabled(bool enabled)
        {
            _isHapticEnabled = enabled;
            PlayerPrefs.SetInt(HAPTIC_PREF_KEY, enabled ? 1 : 0);
            PlayerPrefs.Save();
        }

        public void PlayHaptic()
        {
            if (!_isHapticEnabled)
                return;

#if UNITY_ANDROID && !UNITY_EDITOR
        Handheld.Vibrate();
#elif UNITY_IOS && !UNITY_EDITOR
        // iOS haptic feedback 
        Handheld.Vibrate();
#else
            Debug.Log("Haptic: Vibrate()");
#endif
        }
    }
}