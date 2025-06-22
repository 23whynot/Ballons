using UnityEngine;

namespace CodeBase.Infrastructure.UI.HUD
{
    public class HUDRoot : MonoBehaviour, IHUDRoot
    {
        [SerializeField] private GameObject hubHUD;
        [SerializeField] private GameObject gameHUD;
        
        public void SwitchHUD()
        {
            bool isHubActive = hubHUD.activeSelf;

            hubHUD.SetActive(!isHubActive);
            gameHUD.SetActive(isHubActive);
        }

        public void ShowGameHUD()
        {
            hubHUD.SetActive(false);
            gameHUD.SetActive(true);
        }

        public void ShowHubHUD()
        {
            gameHUD.SetActive(false);
            hubHUD.SetActive(true);
        }
    }
}