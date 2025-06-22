using UnityEngine;
using Zenject;

namespace CodeBase.GamePlay.Ballon.Spawner.Window
{
    public class BalloonSpawnerWindow : MonoBehaviour, IBalloonSpawnerWindow
    {
        private IBallonSpawner _ballonSpawner;

        [Inject]
        public void Construct(IBallonSpawner ballonSpawner) => 
            _ballonSpawner = ballonSpawner;

        private void Awake() => 
            _ballonSpawner.SetBallonWindow(this);

        public RectTransform GetCanvasRect() => 
            GetComponent<RectTransform>();
    }
}