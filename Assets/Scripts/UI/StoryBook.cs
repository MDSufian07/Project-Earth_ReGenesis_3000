using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class StoryBook : MonoBehaviour
    {
        [Header("Story Images")]
        [SerializeField] private Sprite[] pages;

        [Header("UI")]
        [SerializeField] private Image storyImage;
        [SerializeField] private TMP_Text pageText;

        [Header("Buttons")]
        [SerializeField] private Button previousButton;
        [SerializeField] private Button nextButton;

        private int currentPage = 0;

        private void Start()
        {
            previousButton.onClick.AddListener(PreviousPage);
            nextButton.onClick.AddListener(NextPage);

            ShowPage();
        }

        public void NextPage()
        {
            if (currentPage < pages.Length - 1)
            {
                currentPage++;
                ShowPage();
            }
        }

        public void PreviousPage()
        {
            if (currentPage > 0)
            {
                currentPage--;
                ShowPage();
            }
        }

        private void ShowPage()
        {
            if (pages.Length == 0)
                return;

            storyImage.sprite = pages[currentPage];

            if (pageText != null)
                pageText.text = $"Page {currentPage + 1} / {pages.Length}";

            previousButton.interactable = currentPage > 0;
            nextButton.interactable = currentPage < pages.Length - 1;
        }

        public void GoToPage(int pageIndex)
        {
            if (pageIndex < 0 || pageIndex >= pages.Length)
                return;

            currentPage = pageIndex;
            ShowPage();
        }
    }
}