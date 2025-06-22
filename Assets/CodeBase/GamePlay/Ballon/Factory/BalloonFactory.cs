using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.ObjPool.BallonPool;
using Cysharp.Threading.Tasks;
using Zenject;

namespace CodeBase.GamePlay.Ballon.Factory
{
    public class BalloonFactory : IBalloonFactory
    {
        private IBalloonPool _balloonPool;

        [Inject]
        public void Construct(IBalloonPool balloonPool) => 
            _balloonPool = balloonPool;

        public async UniTask<Ballon> CreateBallon() => 
            await _balloonPool.GetFreeBallon(AssetPath.Balloon);
    }
}