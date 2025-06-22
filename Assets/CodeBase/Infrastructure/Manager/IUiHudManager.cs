using Cysharp.Threading.Tasks;

namespace CodeBase.Infrastructure.Manager
{
    public interface IUiHudManager
    {
        UniTask InitializeAsync();
        void SwitchHUD();
        
        void ShowGameHUD();

        void ShowHubHUD();
    }
}