using System.Collections.Generic;
using CodeBase.GamePlay.UI.Animation;
using Cysharp.Threading.Tasks;

namespace CodeBase.GamePlay.Ballon.Controller
{
    public interface IBuyingBalloonController
    {
        bool IsBuyingBalloon(string id);

        void MarkBalloonBuying(string id);

        UniTask InitializeAsync();

        BalloonConfig GetRandomBoughtBalloonConfig();

        BalloonConfigArray GetBalloonConfigArray();
    }
}