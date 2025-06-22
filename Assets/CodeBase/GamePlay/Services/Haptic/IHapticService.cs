using Cysharp.Threading.Tasks;

namespace CodeBase.GamePlay.Services.Haptic
{
    public interface IHapticService
    {
        UniTask InitializeAsync();
        bool IsHapticEnabled { get; }
        void SetHapticEnabled(bool enabled);
        void PlayHaptic();
    }
}