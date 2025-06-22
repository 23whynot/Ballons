using CodeBase.Infrastructure.UI.Window;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.Infrastructure.UI.Elements
{
    public class SwitchWindowButtonOnGui : MonoBehaviour
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
                case TargetWindow.GameHub: _windowManager.OpenWindowAsyncOnGui(WindowAssetsPath.GameHub); break; ;
                case TargetWindow.Leaderboard: _windowManager.OpenWindowAsyncOnGui(WindowAssetsPath.LeaderBoard); break;
                case TargetWindow.Settings: _windowManager.OpenWindowAsyncOnGui(WindowAssetsPath.Settings); break;
                case TargetWindow.EditProfileWindow: _windowManager.OpenWindowAsyncOnGui(WindowAssetsPath.EditProfile); break;
                case TargetWindow.ShopWindow: _windowManager.OpenWindowAsyncOnGui(WindowAssetsPath.ShopWindow); break;
                    default: Debug.LogError("Unknown Window Type"); break;
            }
        }

        private void OnDisable() => 
            button.onClick.RemoveListener(OnClick);
    }
}