using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.GamePlay.ChoiseLevel
{
    public class StarControllerComponent : MonoBehaviour
    {
        [SerializeField] private Image[] stars;

        public void ShowStars(int starsCount)
        {
            HideStars();
            for (int i = 0; i < starsCount; i++)
            {
                stars[i].gameObject.SetActive(true);
            }
        }

        public void HideStars()
        {
            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].gameObject.SetActive(false);
            }
        }
    }
}