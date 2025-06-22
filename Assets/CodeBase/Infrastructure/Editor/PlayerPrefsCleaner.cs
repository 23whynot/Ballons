using UnityEditor;
using UnityEngine;

namespace CodeBase.Infrastructure.Editor
{
    public class PlayerPrefsCleaner : EditorWindow
    {
        [MenuItem("Tools/Clear PlayerPrefs")]
        public static void ShowWindow()
        {
            // Открывает окно очистки PlayerPrefs
            EditorWindow.GetWindow(typeof(PlayerPrefsCleaner), false, "Clear PlayerPrefs");
        }

        private void OnGUI()
        {
            GUILayout.Label("Clear PlayerPrefs", EditorStyles.boldLabel);
            GUILayout.Space(20);

            if (GUILayout.Button("Clear All PlayerPrefs"))
            {
                // Очистить все PlayerPrefs
                PlayerPrefs.DeleteAll();
                Debug.Log("All PlayerPrefs cleared.");
            }

            if (GUILayout.Button("Clear specific PlayerPrefs"))
            {
                // Очистить конкретные PlayerPrefs (например, только те, что используются для RateUs)
                PlayerPrefs.DeleteKey("rate_counter");
                PlayerPrefs.DeleteKey("rate_shown");
                PlayerPrefs.DeleteKey("InterstitialShowCounter");
                PlayerPrefs.DeleteKey("PolicyAccepted");
                Debug.Log("Specific PlayerPrefs cleared.");
            }
        }
    }
}