using System;
using Cysharp.Threading.Tasks;

namespace CodeBase.GamePlay.Ballon.Spawner
{
    public interface IBallonSpawner
    {
        public UniTask InitializeAsync();
        void StartSpawn();

        void StopSpawn();

        void SetBallonWindow(IBalloonSpawnerWindow balloonSpawnerWindow);
    }
}