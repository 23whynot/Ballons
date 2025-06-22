using CodeBase.GamePlay;
using CodeBase.GamePlay.Ballon.Spawner;
using CodeBase.GamePlay.Level.Holder;
using CodeBase.GamePlay.Score;
using CodeBase.GamePlay.Timer;
using CodeBase.Infrastructure.Manager;
using CodeBase.Infrastructure.SceneManagement;
using CodeBase.Infrastructure.UI.LoadingCurtain.Proxy;
using CodeBase.Infrastructure.UI.Window;
using Cysharp.Threading.Tasks;

namespace CodeBase.Infrastructure.States
{
    public class GameplayState : IState
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly ILoadingCurtainProxy _loadingCurtainProxy;
        private readonly IUiHudManager _uiHudManager;
        private readonly IWindowManager _windowManager;
        private readonly ITimerOnLevelController _timerOnLevelController;
        private readonly ILevelHolder _levelHolder;
        private readonly IBallonSpawner _ballonSpawner;
        private readonly IScoreController _scoreController;

        public GameplayState(ISceneLoader sceneLoader,
            ILoadingCurtainProxy loadingCurtainProxy,
            IUiHudManager uiHudManager,
            IWindowManager windowManager,
            ITimerOnLevelController timerOnLevelController,
            ILevelHolder levelHolder,
            IBallonSpawner ballonSpawner,
            IScoreController scoreController)
        {
            _windowManager = windowManager;
            _timerOnLevelController = timerOnLevelController;
            _levelHolder = levelHolder;
            _ballonSpawner = ballonSpawner;
            _scoreController = scoreController;
            _sceneLoader = sceneLoader;
            _loadingCurtainProxy = loadingCurtainProxy;
            _uiHudManager = uiHudManager;
        }
        
        public async UniTask Enter()
        {
            await _loadingCurtainProxy.Show();

            await _windowManager.CloseCurrentWindowAsyncOnGui();
            await _windowManager.CloseCurrentWindowAsyncOnHud();
            _uiHudManager.SwitchHUD();

            await _sceneLoader.LoadScene(InfrastructureAssetPath.GameplayScene);

            _uiHudManager.ShowGameHUD();

            await _loadingCurtainProxy.HideAsync();
            
            StartGame();
        }

        private void StartGame()
        {
            _scoreController.ClearScore();
            _timerOnLevelController.StartTimer(_levelHolder.GetConfig().Time);
            _ballonSpawner.StartSpawn();
        }

        public UniTask Exit() => 
            UniTask.CompletedTask;
    }
}