using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private Color fullHealthColor = Color.green;
    [SerializeField] private Color lowHealthColor = Color.red;
    [SerializeField] private float smoothSpeed = 8f;

    private float targetFill = 1f;

    private void Update()
    {
        fillImage.fillAmount = Mathf.Lerp(
            fillImage.fillAmount,
            targetFill,
            Time.deltaTime * smoothSpeed);

        fillImage.color = Color.Lerp(
            lowHealthColor,
            fullHealthColor,
            fillImage.fillAmount);
    }

    public void SetHealth(float currentHealth, float maxHealth)
    {
        targetFill = currentHealth / maxHealth;
    }
}