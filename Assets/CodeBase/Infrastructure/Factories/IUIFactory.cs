using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Infrastructure.Factories
{
    public interface IUIFactory
    {
        UniTask InitializeAsync();

        GameObject GetGUI();

        GameObject GetHUD();
        
        UniTask<GameObject> CreateGameHubHud();
        
        UniTask<GameObject> CreateGameplayHud();
    }
}