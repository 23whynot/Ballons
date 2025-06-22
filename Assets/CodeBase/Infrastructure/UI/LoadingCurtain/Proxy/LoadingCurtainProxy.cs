using Cysharp.Threading.Tasks;

namespace CodeBase.Infrastructure.UI.LoadingCurtain.Proxy
{
    public class LoadingCurtainProxy :  ILoadingCurtainProxy
    {
        private readonly LoadingCurtain.Factory _factory;
        private LoadingCurtain _loadingCurtain;

        public LoadingCurtainProxy(LoadingCurtain.Factory factory) => 
            _factory = factory;


        public async UniTask InitializeAsync()
        {
            _loadingCurtain = await _factory.Create(InfrastructureAssetPath.LoadingCurtain); 
            _loadingCurtain.gameObject.SetActive(false);
        }

        public async UniTask Show() => 
            await _loadingCurtain.Show();

        public async UniTask HideAsync() => 
            await _loadingCurtain.HideAsync();

        public async UniTask UpdateProgress(float progress) => 
             _loadingCurtain.UpdateProgress(progress);
    }
}