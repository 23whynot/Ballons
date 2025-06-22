using CodeBase.GamePlay.Ballon;
using CodeBase.Infrastructure.AssetManagement;
using Cysharp.Threading.Tasks;
using Zenject;

namespace CodeBase.Infrastructure.ObjPool.BallonPool
{
    public class BallonPool : ObjectPool<Ballon>, IBalloonPool
    {
        public BallonPool(DiContainer container, IAssetProvider assetProvider) : base(container, assetProvider)
        {
        }

        public async UniTask<Ballon> GetFreeBallon(string ballonID) => 
            await GetFreeObject(ballonID);
    }
}