using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AimController : MonoBehaviour
{
    [SerializeField] private Rig aimRig;
    [SerializeField] private float speed = 8f;

    [Header("UI Panels")]
    [SerializeField] private GameObject[] uiPanels;

    private void Start()
    {
        UpdateCursorState();
    }

    private void Update()
    {
        // ESC শুধু Pause Panel Toggle করবে (যদি প্রথম element Pause Panel হয়)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (uiPanels.Length > 0 && uiPanels[0] != null)
            {
                uiPanels[0].SetActive(!uiPanels[0].activeSelf);
            }
        }

        UpdateCursorState();

        // কোনো UI Open থাকলে Aim Update করবে না
        if (IsAnyPanelOpen())
            return;

        bool aiming =
            InputManager.Instance.FireHeld ||
            InputManager.Instance.RepairHeld;

        float targetWeight = aiming ? 1f : 0f;

        aimRig.weight = Mathf.Lerp(
            aimRig.weight,
            targetWeight,
            Time.deltaTime * speed);
    }

    private bool IsAnyPanelOpen()
    {
        foreach (GameObject panel in uiPanels)
        {
            if (panel != null && panel.activeInHierarchy)
                return true;
        }

        return false;
    }

    private void UpdateCursorState()
    {
        if (IsAnyPanelOpen())
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}