using CodeBase.GamePlay.LeaderboardWindow;

namespace CodeBase.GamePlay.UI.Windows
{
    public class Prize
    {
        public PrizeType PrizeType;
        public int Count;

        public Prize(PrizeType prizeType, int count)
        {
            PrizeType = prizeType;
            Count = count;
        }
    }
}