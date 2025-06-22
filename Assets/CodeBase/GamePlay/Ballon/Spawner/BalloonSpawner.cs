using System;
using System.Collections;
using CodeBase.GamePlay.Ballon.Controller;
using CodeBase.GamePlay.Ballon.Factory;
using CodeBase.Infrastructure.CorRun;
using CodeBase.Infrastructure.UI.Window;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CodeBase.GamePlay.Ballon.Spawner
{
    public class BalloonSpawner : IBallonSpawner
    {
        private bool _isSpawning;
        private IWindowManager _windowManager;
        private IBalloonSpawnerWindow _balloonSpawnerWindow;
        private IBalloonFactory _balloonFactory;
        private ICoroutineRunner _coroutineRunner;
        private IBuyingBalloonController _buyingBalloonController;
        private const float SpawnInterval = 0.5f;

        [Inject]
        public void Construct(IWindowManager windowManager,
            IBalloonFactory balloonFactory,
            ICoroutineRunner coroutineRunner,
            IBuyingBalloonController buyingBalloonController)
        {
            _buyingBalloonController = buyingBalloonController; 
            _coroutineRunner = coroutineRunner;
            _balloonFactory = balloonFactory;
            _windowManager = windowManager;
        }

        public async UniTask InitializeAsync()
        {
            await _windowManager.OpenWindowAsyncOnHUD(WindowAssetsPath.GamePlayWindow);
            await _windowManager.CloseWindowByIdOnHud(WindowAssetsPath.GamePlayWindow);
        }

        public void SetBallonWindow(IBalloonSpawnerWindow balloonSpawnerWindow) => 
            _balloonSpawnerWindow = balloonSpawnerWindow;

        public void StartSpawn()
        {
            if (_isSpawning)
                return;

            _windowManager.OpenWindowAsyncOnGui(WindowAssetsPath.GamePlayWindow);
            _isSpawning = true;
            _coroutineRunner.StartCoroutine(SpawnLoop());
        }

        public void StopSpawn()
        {
            _isSpawning = false; 
            _windowManager.CloseWindowByIdOnHud(WindowAssetsPath.GamePlayWindow);
        }

        private IEnumerator SpawnLoop()
        {
            while (_isSpawning)
            {
                SpawnOne().Forget();
                yield return new WaitForSeconds(SpawnInterval);
            }
        }

        private async UniTaskVoid SpawnOne()
        {
            var balloon = await _balloonFactory.CreateBallon();
            balloon.Initialize(_buyingBalloonController.GetRandomBoughtBalloonConfig());
            
            balloon.transform.SetParent(_balloonSpawnerWindow.GetCanvasRect(), worldPositionStays: false);

            RectTransform canvasRect = _balloonSpawnerWindow.GetCanvasRect();
            RectTransform balloonRect = balloon.GetComponent<RectTransform>();

            float halfWidth = canvasRect.rect.width / 2f;
            float x = UnityEngine.Random.Range(-halfWidth, halfWidth); // случайное значение по ширине
            float y = -canvasRect.rect.height / 2f - 100f; // чуть ниже

            balloonRect.anchoredPosition = new Vector2(x, y);
        }


    }
}
