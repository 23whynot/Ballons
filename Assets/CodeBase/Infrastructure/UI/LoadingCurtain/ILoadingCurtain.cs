using Cysharp.Threading.Tasks;

namespace CodeBase.Infrastructure.UI.LoadingCurtain
{
    public interface ILoadingCurtain
    {
        UniTask Show();

        UniTask HideAsync();

        UniTask UpdateProgress(float progress);
    }
}