using Cysharp.Threading.Tasks;

namespace CodeBase.Infrastructure.UI.LoadingCurtain.Proxy
{
    public interface ILoadingCurtainProxy
    {
        UniTask InitializeAsync();
        
        UniTask Show();

        UniTask HideAsync();

        UniTask UpdateProgress(float progress);
    }
}