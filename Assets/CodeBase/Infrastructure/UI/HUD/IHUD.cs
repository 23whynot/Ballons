using Cysharp.Threading.Tasks;

namespace CodeBase.Infrastructure.UI.HUD
{
    public interface IHUD
    {
        UniTask ActivateAsync();
    }
}