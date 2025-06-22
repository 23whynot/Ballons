using CodeBase.Infrastructure.Factories;
using CodeBase.Infrastructure.UI.HUD;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.Manager
{
    public class UiHudManager : IUiHudManager
    {
        private IHUDRoot _hudRoot;

        private IUIFactory _factory;

        [Inject]
        public void Construct(IUIFactory factory) =>
            _factory = factory;


        public async UniTask InitializeAsync()
        {
            GameObject _hud = _factory.GetHUD();
            _hudRoot = _hud.GetComponent<IHUDRoot>();
        }

        public void SwitchHUD() =>
            _hudRoot.SwitchHUD();

        public void ShowGameHUD() =>
            _hudRoot.ShowGameHUD();

        public void ShowHubHUD() =>
            _hudRoot.ShowHubHUD();
    }
}