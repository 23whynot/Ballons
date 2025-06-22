using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.GamePlay.Window.LeaderBoard
{
    public class LeaderBoardPlace : MonoBehaviour
    {
        [SerializeField] private Image  avatarImage;
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private Sprite botSprite;

        public void Initialize(string name, int score, bool IsPlayer = false)
        {
            nameText.text = name;
            scoreText.text = score.ToString();

            if (IsPlayer)
            {
                image.color = new Color(0.4588f, 0.7804f, 1f, 1f); // #75C7FF
            }
            else
            {
                avatarImage.sprite = botSprite;
            }
        }

        public void SetAvatar(Sprite playerAvatar) => 
            avatarImage.sprite = playerAvatar;
    }
}