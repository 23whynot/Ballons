using CodeBase.Infrastructure.AssetManagement;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.Factories
{
    public class UIFactory : IUIFactory
    {
        private GameObject hud;
        private GameObject gui;
        
        
        private IAssetProvider _assetProvider;
        private DiContainer _container;

        [Inject]
        public void Construct(IAssetProvider assetProvider, DiContainer container)
        {
            _assetProvider = assetProvider;
            _container = container;
        }

        public async UniTask InitializeAsync()
        {
            await CreateGUI();
            await CreateHUD();
        }

        public GameObject GetGUI() => 
            gui;

        public GameObject GetHUD() => 
            hud;

        private async UniTask CreateHUD()
        {
            GameObject obj = await _assetProvider.Load<GameObject>(AssetPath.HUD);
            GameObject hudInstance = _container.InstantiatePrefab(obj);

            hud = hudInstance;
        }

        private async UniTask CreateGUI()
        {
            GameObject obj = await _assetProvider.Load<GameObject>(AssetPath.GUI);
            GameObject guiInstance = _container.InstantiatePrefab(obj);
            
            gui = guiInstance;
        }

        public async UniTask<GameObject> CreateGameHubHud()
        {
            GameObject hubHud = await _assetProvider.Load<GameObject>(AssetPath.GameHubHud);
            GameObject hubHudInstans = _container.InstantiatePrefab(hubHud);
            
            hubHudInstans.SetActive(false);
            
            return hubHudInstans;
        }

        public async UniTask<GameObject> CreateGameplayHud()
        {
            GameObject gameplayHud = await _assetProvider.Load<GameObject>(AssetPath.GameplayHud);
            GameObject gameplayHudInstans = _container.InstantiatePrefab(gameplayHud);
            
            gameplayHudInstans.SetActive(false);
            
            return gameplayHudInstans;
        }
    }
}