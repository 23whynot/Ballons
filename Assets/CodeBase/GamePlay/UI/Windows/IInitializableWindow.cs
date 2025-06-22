using CodeBase.Infrastructure.UI.Window;

namespace CodeBase.GamePlay.UI.Windows
{
    public interface IInitializableWindow<T> : IWindow
    {
        void Initialize(T parametr);
    }
}