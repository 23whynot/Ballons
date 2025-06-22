using CodeBase.GamePlay.Ballon;
using UnityEngine;

namespace CodeBase.GamePlay.UI.Animation
{
    [CreateAssetMenu(fileName = "BallConfig", menuName = "Shop/BallConfigArray")]
    public class BalloonConfigArray : ScriptableObject
    {
        public BalloonConfig[] ballonConfigs;
    }
}