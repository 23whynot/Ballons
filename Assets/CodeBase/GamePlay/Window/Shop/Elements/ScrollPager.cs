using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.GamePlay.Window.Shop.Elements
{
    public class ScrollPager : MonoBehaviour
    {
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private Button nextButton;
        [SerializeField] private Button prevButton;

        private int currentPage = 0;
        private int totalPages = 1;
        private float viewportWidth;

        private void Start()
        {
            viewportWidth = scrollRect.viewport.rect.width;
            if (viewportWidth <= 0f)
                viewportWidth = 1f;

            nextButton.onClick.AddListener(OnNextPage);
            prevButton.onClick.AddListener(OnPrevPage);
            UpdateButtons();
        }

        public void SetTotalPages(int pages)
        {
            totalPages = Mathf.Max(1, pages);
            currentPage = 0;
            UpdateButtons();
            ScrollToPage(currentPage, instant: true);
        }

        private void OnNextPage()
        {
            if (currentPage >= totalPages - 1) return;
            currentPage++;
            ScrollToPage(currentPage);
            UpdateButtons();
        }

        private void OnPrevPage()
        {
            if (currentPage <= 0) return;
            currentPage--;
            ScrollToPage(currentPage);
            UpdateButtons();
        }

        private void ScrollToPage(int page, bool instant = false)
        {
            if (totalPages <= 1)
            {
                scrollRect.horizontalNormalizedPosition = 0f;
                return;
            }

            float target = 1f - ((float)page / (totalPages - 1));
            target = Mathf.Clamp01(target);

            if (instant)
                scrollRect.horizontalNormalizedPosition = target;
            else
                scrollRect.DOHorizontalNormalizedPos(target, 0.4f).SetEase(Ease.InOutSine);
        }

        private void UpdateButtons()
        {
            prevButton.interactable = currentPage > 0;
            nextButton.interactable = currentPage < totalPages - 1;
        }
    }
}
