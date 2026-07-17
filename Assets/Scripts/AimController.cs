using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AimController : MonoBehaviour
{
    public Rig aimRig;
    public float speed = 8f;

    private bool _cursorLocked = false;

    void Update()
    {
        // Lock & Hide cursor on first left/right click
        if (!_cursorLocked && (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)))
        {
            LockCursor();
        }

        // Unlock & Show cursor on ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnlockCursor();
        }

        // Aim Rig
        float targetWeight = (Input.GetMouseButton(0) || Input.GetMouseButton(1)) ? 1f : 0f;
        aimRig.weight = Mathf.Lerp(aimRig.weight, targetWeight, Time.deltaTime * speed);
    }

    void LockCursor()
    {
        _cursorLocked = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void UnlockCursor()
    {
        _cursorLocked = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}