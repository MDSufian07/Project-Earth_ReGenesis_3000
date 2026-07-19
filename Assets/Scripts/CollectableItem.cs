using System.Collections;
using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private CanvasGroup pressEUI;

    [Header("Mission")]
    [SerializeField] private GameObject missionCompletePanel;

    [Header("Settings")]
    [SerializeField] private float destroyDelay = 0.2f;

    [HideInInspector]
    public bool playerInRange = false;

    private void Awake()
    {
        // Keep the UI object active, but invisible.
        if (pressEUI != null)
        {
            pressEUI.alpha = 0f;
            pressEUI.interactable = false;
            pressEUI.blocksRaycasts = false;
        }

        if (missionCompletePanel != null)
            missionCompletePanel.SetActive(false);
    }

    public void ShowPrompt(bool show)
    {
        playerInRange = show;

        if (pressEUI == null)
            return;

        pressEUI.alpha = show ? 1f : 0f;
        pressEUI.interactable = show;
        pressEUI.blocksRaycasts = show;
    }

    public void Collect()
    {
        StartCoroutine(CollectRoutine());
    }

    private IEnumerator CollectRoutine()
    {
        ShowPrompt(false);

        if (missionCompletePanel != null)
            missionCompletePanel.SetActive(true);

        yield return new WaitForSeconds(destroyDelay);

        Destroy(gameObject);
    }
}