using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Infrastructure.ScriptblObj
{
    [CreateAssetMenu(menuName = "ScriptableObject/Sprite List")]
    public class SpriteList : ScriptableObject
    {
        public List<SpriteObject> sprites;
    }
}
