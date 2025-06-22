using UnityEngine;

namespace CodeBase.Infrastructure.ScriptblObj
{
    [CreateAssetMenu(menuName = "ScriptableObject/Sprite")]
    public class SpriteObject : ScriptableObject
    {
        public Sprite sprite;
    }
}