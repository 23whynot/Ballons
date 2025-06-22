using CodeBase.Infrastructure.UI.Window;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.Infrastructure.UI.Elements
{
    public class SwitchWindowButtonOnHud : MonoBehaviour
    {
        [SerializeField] private TargetWindow targetWindow = 0;
        [SerializeField] private Button button;

        private IWindowManager _windowManager;

        [Inject]
        public void Construct(IWindowManager windowManager) => 
            _windowManager = windowManager;

        private void OnEnable() => 
            button.onClick.AddListener(OnClick);

        private void OnClick()
        {
            switch (targetWindow)
            {
                case TargetWindow.GameHub: _windowManager.OpenWindowAsyncOnHUD(WindowAssetsPath.GameHub); break;
                case TargetWindow.SetLevelWindow: _windowManager.OpenWindowAsyncOnHUD(WindowAssetsPath.SetLevelWindow); break;
            }
        }

        private void OnDisable() => 
            button.onClick.RemoveListener(OnClick);
    }
}