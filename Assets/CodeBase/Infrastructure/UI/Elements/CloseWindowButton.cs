using CodeBase.Infrastructure.UI.Window;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.Infrastructure.UI.Elements
{
    public class CloseWindowButton : MonoBehaviour
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private bool Gui;
        [SerializeField] private bool Hud;
        
        private IWindowManager _windowManager;
        
        [Inject]
        public void Construct(IWindowManager windowManager) => 
            _windowManager = windowManager;

        private void OnEnable() => closeButton.onClick.AddListener(OnClick);

        private void OnClick()
        {
            //SoundMaster.Instance.SoundPlayClick(0f);
            if (Gui)
            {
                _windowManager.CloseCurrentWindowAsyncOnGui();
            }
            
            if (Hud)
            {
                _windowManager.CloseCurrentWindowAsyncOnHud();
            }
        }

        private void OnDisable() => 
            closeButton.onClick.RemoveListener(OnClick);
    }
}