using Cysharp.Threading.Tasks;

namespace CodeBase.GamePlay.Ballon.Factory
{
    public interface IBalloonFactory
    { 
        UniTask<Ballon> CreateBallon();
    }
}