using System.Threading.Tasks;
using CodeBase.GamePlay;
using CodeBase.GamePlay.Ballon.Spawner;
using CodeBase.Infrastructure.Manager;
using CodeBase.Infrastructure.SceneManagement;
using CodeBase.Infrastructure.UI.LoadingCurtain.Proxy;
using CodeBase.Infrastructure.UI.Window;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
    public class GameHubState : IState
    {
        private readonly ILoadingCurtainProxy _loadingCurtain;
        private readonly ISceneLoader _sceneLoader;
        private readonly IWindowManager _windowManager;
        private readonly IUiHudManager _uiHudManager;
        private readonly IBallonSpawner _ballonSpawner;

        private const string HowToPlayShownKey = "HowToPlay_Shown";

        public GameHubState(ILoadingCurtainProxy loadingCurtain,
            ISceneLoader sceneLoader,
            IWindowManager windowManager,
            IUiHudManager uiHudManager,
            IBallonSpawner ballonSpawner)
        {
            _loadingCurtain = loadingCurtain;
            _sceneLoader = sceneLoader;
            _windowManager = windowManager;
            _uiHudManager = uiHudManager;
            _ballonSpawner = ballonSpawner;
        }

        public async UniTask Enter()
        {
            await _loadingCurtain.Show();

            await _sceneLoader.LoadScene(InfrastructureAssetPath.GameHubScene);
            await _loadingCurtain.UpdateProgress(50);

            await StopGamePlay();

            await _windowManager.OpenWindowAsyncOnHUD(WindowAssetsPath.GameHub);

            if (PlayerPrefs.GetInt(HowToPlayShownKey, 0) == 0)
            {
                _windowManager.OpenWindowAsyncOnGui(WindowAssetsPath.HowToPlayWindow);
            }

            await _loadingCurtain.HideAsync();
        }

        private async UniTask StopGamePlay()
        {
            _uiHudManager.ShowHubHUD();
            _ballonSpawner.StopSpawn();
        }


        public UniTask Exit() =>
            UniTask.CompletedTask;
    }
}