using UnityEngine;

namespace New_Folder
{
    public class MouseMoveAndRotate : MonoBehaviour
    {
        [Header("Movement")]
        public float moveSpeed = 3f;
        public float moveLimit = 3f;      // ±3 units

        [Header("Rotation")]
        public float rotationLimit = 30f; // ±30 degrees

        private float startY;
        private Quaternion startRotation;

        void Start()
        {
            startY = transform.position.y;
            startRotation = transform.localRotation;
        }

        void Update()
        {
            float mouseY = Input.GetAxis("Mouse Y");

            // Move on World Y
            Vector3 pos = transform.position;
            pos.y += mouseY * moveSpeed * Time.deltaTime;
            pos.y = Mathf.Clamp(pos.y, startY - moveLimit, startY + moveLimit);
            transform.position = pos;

            // Calculate movement percentage (-1 to +1)
            float normalized = (pos.y - startY) / moveLimit;

            // Convert to rotation (-30 to +30)
            float zRotation = normalized * rotationLimit;

            // Apply relative to starting rotation
            transform.localRotation = startRotation * Quaternion.Euler(0f, 0f, zRotation);
        }
    }
}