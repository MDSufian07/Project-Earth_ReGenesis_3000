using TMPro;
using UnityEngine;

namespace UI
{
    public class ObjectiveIndicator : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Transform player;
        [SerializeField] private Transform target;

        [Header("UI")]
        [SerializeField] private RectTransform arrow;
        [SerializeField] private TextMeshProUGUI distanceText;
        [SerializeField] private float extraRotation = 90f; 

        [Header("Settings")]
        [SerializeField] private float hideDistance = 5f;

        private void Start()
        {
            if (mainCamera == null)
                mainCamera = Camera.main;
        }

        private void Update()
        {
            
            if (player == null || target == null || arrow == null)
                return;

            // Distance
            float distance = Vector3.Distance(player.position, target.position);

            if (distanceText != null)
                distanceText.text = $"Item Distance : {Mathf.RoundToInt(distance)} m";

            // Hide when close
            bool hide = distance <= hideDistance;

            arrow.gameObject.SetActive(!hide);

            if (distanceText != null)
                distanceText.gameObject.SetActive(!hide);

            if (hide)
                return;

            // Direction to target
            Vector3 direction = target.position - player.position;
            direction.y = 0f;

            // Signed angle between Player Forward and Target
            float angle = Vector3.SignedAngle(
                player.forward,
                direction.normalized,
                Vector3.up);

            // Rotate Arrow
            arrow.localEulerAngles = new Vector3(60f, 0f, -angle + extraRotation);

            // Debug
            // Debug.Log($"Angle : {angle}");
        }

        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
        }
    }
}