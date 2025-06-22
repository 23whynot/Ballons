using CodeBase.GamePlay.Audio;
using CodeBase.GamePlay.Ballon.Controller;
using CodeBase.GamePlay.Ballon.Factory;
using CodeBase.GamePlay.Ballon.Spawner;
using CodeBase.GamePlay.Controllers.AvatarsController;
using CodeBase.GamePlay.Currency;
using CodeBase.GamePlay.Level.Controller;
using CodeBase.GamePlay.Level.Holder;
using CodeBase.GamePlay.Score;
using CodeBase.GamePlay.Services.Haptic;
using CodeBase.GamePlay.Services.NickName;
using CodeBase.GamePlay.Timer;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.CorRun;
using CodeBase.Infrastructure.Factories;
using CodeBase.Infrastructure.Manager;
using CodeBase.Infrastructure.ObjPool.BallonPool;
using CodeBase.Infrastructure.SceneManagement;
using CodeBase.Infrastructure.Services.Player;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Infrastructure.States;
using CodeBase.Infrastructure.UI.LoadingCurtain;
using CodeBase.Infrastructure.UI.LoadingCurtain.Proxy;
using CodeBase.Infrastructure.UI.Window;
using Cysharp.Threading.Tasks;
using Zenject;

namespace CodeBase.Infrastructure.CompositionRoot
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindInfrastructure();

            BindGamePlay();
        }

        private void BindGamePlay()
        {
            Container.Bind<INicknameService>().To<NicknameService>().AsSingle();

            Container.Bind<IHapticService>().To<HapticService>().AsSingle();
            
            Container.Bind<IAvatarController>().To<AvatarController>().AsSingle();
            
            Container.Bind<IAudioService>().To<AudioService>().AsSingle();
            
            Container.Bind<ILevelHolder>().To<LevelHolder>().AsSingle();
            
            Container.Bind<ITimerOnLevelController>().To<TimerOnLevelController>().AsSingle();
            
            Container.Bind<IBalloonPool>().To<BallonPool>().AsSingle();
            
            Container.Bind<IBalloonFactory>().To<BalloonFactory>().AsSingle();
            
            Container.Bind<IBallonSpawner>().To<BalloonSpawner>().AsSingle();
            
            Container.Bind<IScoreController>().To<ScoreController>().AsSingle();
            
            Container.Bind<ICurrencyController>().To<CurrencyController>().AsSingle();
            
            Container.Bind<IBuyingBalloonController>().To<BuyingBalloonController>().AsSingle();
            
            Container.Bind<ILevelDataController>().To<LevelDataController>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<PlayerNameService>().AsSingle(); 
        }

        private void BindInfrastructure()
        {
            
            Container.Bind<IGameStateMachine>().To<GameStateMachine>().AsSingle();
            
            Container.Bind<IStateFactory>().To<StateFactory>().AsSingle();

            Container.Bind<IAssetProvider>().To<AssetProvider>().AsSingle();
            
            Container.Bind<IWindowManager>().To<WindowManager>().AsSingle();

            Container.Bind<IUIFactory>().To<UIFactory>().AsSingle();
            
            Container.Bind<IUiHudManager>().To<UiHudManager>().AsSingle();
            
            Container.Bind<ISaveLoadService>().To<SaveLoadService>().AsSingle();
            
            Container.BindInterfacesTo<SceneLoader>().AsSingle();

            BindSDK();
            
            BindCoroutineRunner();
            
            BindLoadingCurtain();
            
            BindGameBootstrapper();
        }

        private void BindSDK()
        {

        }

        private void BindCoroutineRunner()
        {
            Container.Bind<ICoroutineRunner>()
                .FromComponentInNewPrefabResource(InfrastructureAssetPath.CoroutineRunner)  
                .AsSingle();
        }

        private void BindLoadingCurtain()
        {
            Container.BindFactory<string, UniTask<LoadingCurtain>, LoadingCurtain.Factory>()
                .FromFactory<PrefabFactoryAsync<LoadingCurtain>>();
            
            Container.Bind<ILoadingCurtainProxy>().To<LoadingCurtainProxy>().AsSingle();
        }

        private void BindGameBootstrapper()
        {
            Container.BindFactory
                    <GameBootstrapper, GameBootstrapper.Factory>()
                .FromComponentInNewPrefabResource(InfrastructureAssetPath.GameBootstrapper);
        }
    }
}