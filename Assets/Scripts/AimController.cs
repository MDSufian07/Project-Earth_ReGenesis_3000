using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AimController : MonoBehaviour
{
    [SerializeField] private Rig aimRig;
    [SerializeField] private float speed = 8f;

    private bool cursorLocked;

    private void Update()
    {
        if (!cursorLocked &&
            (InputManager.Instance.FirePressed ||
             InputManager.Instance.RepairPressed))
        {
            LockCursor();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnlockCursor();
        }

        bool aiming =
            InputManager.Instance.FireHeld ||
            InputManager.Instance.RepairHeld;

        float targetWeight = aiming ? 1f : 0f;

        aimRig.weight = Mathf.Lerp(
            aimRig.weight,
            targetWeight,
            Time.deltaTime * speed);
    }

    private void LockCursor()
    {
        cursorLocked = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void UnlockCursor()
    {
        cursorLocked = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}