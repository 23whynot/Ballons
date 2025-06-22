using CodeBase.GamePlay.Audio;
using CodeBase.GamePlay.Ballon.Controller;
using CodeBase.GamePlay.Ballon.Spawner;
using CodeBase.GamePlay.Controllers.AvatarsController;
using CodeBase.GamePlay.Currency;
using CodeBase.GamePlay.Level.Controller;
using CodeBase.GamePlay.Services;
using CodeBase.GamePlay.Services.Haptic;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factories;
using CodeBase.Infrastructure.Manager;
using CodeBase.Infrastructure.Services.Player;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Infrastructure.UI.LoadingCurtain.Proxy;
using CodeBase.Infrastructure.UI.Window;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
    public class GameBootstrapState : IState
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IGameStateMachine _gameStateMachine;
        private readonly ILoadingCurtainProxy _loadingCurtainProxy;
        private readonly IUiHudManager _hudManager;
        private readonly IUIFactory _uiFactory;
        private readonly IWindowManager _windowManager;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IAvatarController _avatarController;
        private readonly IPlayerNameService _playerNameService;
        private readonly IHapticService _hapticService;
        private readonly bool _isPolicyAccepted;
        private readonly IAudioService _audioService;
        private readonly IBallonSpawner _ballonSpawner;
        private readonly ICurrencyController _currencyController;
        private readonly IBuyingBalloonController _buyingBalloonController;
        private readonly ILevelDataController _levelDataController;

        public GameBootstrapState(IAssetProvider assetProvider,
            IGameStateMachine gameStateMachine,
            ILoadingCurtainProxy loadingCurtainProxy,
            IUiHudManager hudManager,
            IUIFactory uiFactory,
            IWindowManager windowManager,
            ISaveLoadService saveLoadService,
            IAvatarController avatarController,
            IPlayerNameService playerNameService,
            IHapticService hapticService,
            IAudioService audioService,
            IBallonSpawner ballonSpawner,
            ICurrencyController currencyController,
            IBuyingBalloonController buyingBalloonController,
            ILevelDataController levelDataController)
        {
            _currencyController = currencyController;
            _buyingBalloonController = buyingBalloonController;
            _levelDataController = levelDataController;
            _hudManager = hudManager;
            _assetProvider = assetProvider;
            _gameStateMachine = gameStateMachine;
            _loadingCurtainProxy = loadingCurtainProxy;
            _uiFactory = uiFactory;
            _windowManager = windowManager;
            _saveLoadService = saveLoadService;
            _avatarController = avatarController;
            _playerNameService = playerNameService;
            _hapticService = hapticService;
            _audioService = audioService;
            _ballonSpawner = ballonSpawner;
        }

        public async UniTask Enter()
        {
            await InitServices();
            await _gameStateMachine.Enter<GameHubState>();
        }

        private async UniTask InitServices()
        {
            //First Initialize
            await _assetProvider.InitializeAsync();
            await _saveLoadService.InitializeAsync();
            
            //Second Initialize
            await _avatarController.InitializeAsync();
            await _uiFactory.InitializeAsync();
            await _playerNameService.InitializeAsync();
            await _hapticService.InitializeAsync();
            await _windowManager.InitializeAsync();
            await _loadingCurtainProxy.InitializeAsync();
            await _hudManager.InitializeAsync();
            await _audioService.InitializeAsync();
            await _ballonSpawner.InitializeAsync();
            await _buyingBalloonController.InitializeAsync();
            await _levelDataController.InitializeAsync();

            //Third Initialize
            await _currencyController.InitializeAsync();
        }

        public UniTask Exit() =>
            UniTask.CompletedTask;
    }
}