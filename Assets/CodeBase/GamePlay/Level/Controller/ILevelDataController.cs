using Cysharp.Threading.Tasks;

namespace CodeBase.GamePlay.Level.Controller
{
    public interface ILevelDataController
    {
        UniTask InitializeAsync();

        void MarkLevelAsUnlocked(int level);

        void MarkLevelAsComplete(int level, int starsCount);

        public bool IsLevelUnlocked(int level);

        int GetStarsCount(int level);
    }
}