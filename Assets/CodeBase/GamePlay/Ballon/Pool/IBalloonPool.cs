using CodeBase.GamePlay.Ballon;
using Cysharp.Threading.Tasks;

namespace CodeBase.Infrastructure.ObjPool.BallonPool
{
    public interface IBalloonPool
    {
        UniTask<Ballon> GetFreeBallon(string ballonID);
    }
}