using CodeBase.GamePlay.UI.Animation;
using CodeBase.GamePlay.Window.Shop.Elements;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.GamePlay.Window.Shop.Animation
{
    public class ShopUIAnimator : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private RectTransform ballContainer;  // Исходный контейнер (пустой или с первым набором)
        [SerializeField] private ShopPlate ballSlotPrefab;
        [SerializeField] private BalloonConfigArray balloonConfigArray;
        [SerializeField] private Button nextButton;
        [SerializeField] private Button prevButton;

        [Header("Layout Settings")]
        [SerializeField] private int ballsPerPage = 4;
        [SerializeField] private int columns = 2;
        [SerializeField] private float spacingX = 40f;
        [SerializeField] private float spacingY = 40f;
        [SerializeField] private Vector2 cellSize = new Vector2(200f, 200f);

        [Header("Animation")]
        [SerializeField] private float animationDuration = 0.4f;

        private int currentPage = 0;
        private bool isAnimating = false;

        private void Start()
        {
            nextButton.onClick.AddListener(OnNextPage);
            prevButton.onClick.AddListener(OnPrevPage);
            LoadPage(currentPage, ballContainer);
        }

        public void OnNextPage()
        {
            if (isAnimating) return;
            int nextPage = currentPage + 1;
            if (nextPage * ballsPerPage >= balloonConfigArray.ballonConfigs.Length) return;
            AnimatePage(nextPage, Vector2.left);
        }

        public void OnPrevPage()
        {
            if (isAnimating) return;
            int prevPage = currentPage - 1;
            if (prevPage < 0) return;
            AnimatePage(prevPage, Vector2.right);
        }

        private void LoadPage(int page, RectTransform container)
        {
            // Удаляем все слоты из контейнера
            foreach (Transform child in container)
                Destroy(child.gameObject);

            for (int i = 0; i < ballsPerPage; i++)
            {
                int index = page * ballsPerPage + i;
                if (index >= balloonConfigArray.ballonConfigs.Length) break;

                var slot = Instantiate(ballSlotPrefab, container);
                slot.Initialize(balloonConfigArray.ballonConfigs[index].Icon);

                var rect = slot.GetComponent<RectTransform>();
                PositionSlot(rect, i);
            }
        }

        private void AnimatePage(int targetPage, Vector2 direction)
        {
            if (isAnimating) return;
            isAnimating = true;

            // Создаём новый пустой контейнер
            RectTransform newContainer = new GameObject("BallContainer_New", typeof(RectTransform)).GetComponent<RectTransform>();
            newContainer.SetParent(ballContainer.parent, false); // false — сохранить локальные transform

            // Настраиваем anchor и pivot для удобного позиционирования (левый верхний угол)
            newContainer.anchorMin = new Vector2(0, 1);
            newContainer.anchorMax = new Vector2(0, 1);
            newContainer.pivot = new Vector2(0, 1);

            // Смещаем новый контейнер в сторону, откуда будет появляться
            newContainer.anchoredPosition = direction * ballContainer.rect.width;

            // Заполняем новый контейнер элементами нужной страницы
            LoadPage(targetPage, newContainer);

            // Создаём анимацию смещения контейнеров
            Sequence seq = DOTween.Sequence();
            seq.Join(ballContainer.DOAnchorPos(-direction * ballContainer.rect.width, animationDuration));
            seq.Join(newContainer.DOAnchorPos(Vector2.zero, animationDuration));

            seq.OnComplete(() =>
            {
                Destroy(ballContainer.gameObject);
                ballContainer = newContainer;
                currentPage = targetPage;
                isAnimating = false;
            });
        }

        private void PositionSlot(RectTransform slotRect, int indexInPage)
        {
            int row = indexInPage / columns;
            int col = indexInPage % columns;

            float x = col * (cellSize.x + spacingX);
            float y = -row * (cellSize.y + spacingY); // движение вниз по Y

            // Устанавливаем anchor и pivot слота (левый верхний угол)
            slotRect.anchorMin = new Vector2(0, 1);
            slotRect.anchorMax = new Vector2(0, 1);
            slotRect.pivot = new Vector2(0, 1);

            slotRect.anchoredPosition = new Vector2(x, y);
        }
    }
}
