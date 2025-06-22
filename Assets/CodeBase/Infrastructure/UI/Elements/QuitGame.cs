using System;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Infrastructure.UI.Elements
{
    public class QuitGame : MonoBehaviour
    {
        [SerializeField] private Button quitButton;

        private void Awake() => 
            quitButton.onClick.AddListener(ExitGame);

        private void OnDestroy() => 
            quitButton.onClick.RemoveListener(ExitGame);

        public void ExitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // Завершение игры в билде
#endif
        }
    }
}