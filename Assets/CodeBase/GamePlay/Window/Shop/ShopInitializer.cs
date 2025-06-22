using CodeBase.GamePlay.Ballon.Controller;
using CodeBase.GamePlay.UI.Animation;
using CodeBase.GamePlay.Window.Shop.Elements;
using UnityEngine;
using Zenject;

namespace CodeBase.GamePlay.Window.Shop
{
    public class ShopInitializer : MonoBehaviour
    {
        [SerializeField] private PageShop shopPagePrefab;  
        [SerializeField] private Transform content;   
        [SerializeField] private ScrollPager scrollPager;

        [SerializeField] private int ballsPerPage = 4;
        private DiContainer _container;
        private IBuyingBalloonController _buyingBalloonController;

        [Inject]
        public void Construct(DiContainer container,
            IBuyingBalloonController buyingBalloonController)
        {
            _buyingBalloonController = buyingBalloonController;
            _container = container;
        }

        private void Awake()
        {
            var totalBalls = _buyingBalloonController.GetBalloonConfigArray().ballonConfigs.Length;
            int totalPages = Mathf.CeilToInt((float)totalBalls / ballsPerPage);

            for (int pageIndex = 0; pageIndex < totalPages; pageIndex++)
            {
                var pageInstance = _container.InstantiatePrefab(shopPagePrefab, content);
                var shopPlates = pageInstance.GetComponent<PageShop>().GetShopPlates();

                int startIndex = pageIndex * ballsPerPage;

                for (int i = 0; i < shopPlates.Length; i++)
                {
                    int spriteIndex = startIndex + i;

                    if (spriteIndex < totalBalls)
                    {
                        shopPlates[i].Initialize(_buyingBalloonController.GetBalloonConfigArray().ballonConfigs[spriteIndex].Icon);
                        shopPlates[i].Activate();
                    }
                    else
                    {
                        shopPlates[i].Deactivate();
                    }
                }
            }

            scrollPager.SetTotalPages(totalPages);
        }

    }
}