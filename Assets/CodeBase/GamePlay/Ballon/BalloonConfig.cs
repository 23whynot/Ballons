using UnityEngine;

namespace CodeBase.GamePlay.Ballon
{
    [CreateAssetMenu(fileName = "BallConfig", menuName = "Shop/BallConfig")]
    public class BalloonConfig : ScriptableObject
    {
        public Sprite Icon;
        public int score;
    }
}