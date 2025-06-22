using System;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Infrastructure.UI.Elements
{
    public class HowToPlayOkButton : MonoBehaviour
    {
        [SerializeField] private Button okButton;

        private const string HowToPlayShownKey = "HowToPlay_Shown";

        private void Awake() => 
            okButton.onClick.AddListener(SetHowToPlayShown);

        private void OnDestroy() => 
            okButton.onClick.RemoveListener(SetHowToPlayShown);

        private void SetHowToPlayShown()
        {
            PlayerPrefs.SetInt(HowToPlayShownKey, 1);
            PlayerPrefs.Save();
        }
    }
}