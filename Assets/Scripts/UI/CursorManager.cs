using UnityEngine;

namespace UI
{
    public class CursorManager : MonoBehaviour
    {
        [SerializeField] private bool lockCursorOnStart = true;

        private void Start()
        {
            Cursor.lockState = lockCursorOnStart
                ? CursorLockMode.Locked
                : CursorLockMode.None;

            Cursor.visible = !lockCursorOnStart;
        }
    }
}