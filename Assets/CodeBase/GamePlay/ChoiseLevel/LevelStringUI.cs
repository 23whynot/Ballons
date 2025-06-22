using CodeBase.GamePlay.Level.Holder;
using UnityEngine;

namespace CodeBase.GamePlay.ChoiseLevel
{
    public class LevelStringUI : MonoBehaviour
    {
        [SerializeField] private LevelUI[] _levels;
        
        public LevelUI[] Levels => _levels;
    }
}