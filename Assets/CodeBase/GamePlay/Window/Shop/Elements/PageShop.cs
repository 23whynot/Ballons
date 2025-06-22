using UnityEngine;

namespace CodeBase.GamePlay.Window.Shop.Elements
{
    public class PageShop : MonoBehaviour
    {
        [SerializeField] private ShopPlate[] shopPlates;

        public ShopPlate[] GetShopPlates() => 
            shopPlates;
    }
}