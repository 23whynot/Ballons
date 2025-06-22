using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.GamePlay.UI.Elements
{
    public class ButtonClickSound : MonoBehaviour
    {
        [SerializeField] private Button button;

        private void Awake() => 
            button.onClick.AddListener(PlaySound);

        private void PlaySound()
        {
            //SoundMaster.Instance.SoundPlayClick(0f);
        }

        private void OnDestroy() => 
            button.onClick.RemoveListener(PlaySound);
    }
}