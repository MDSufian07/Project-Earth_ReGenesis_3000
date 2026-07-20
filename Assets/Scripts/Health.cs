using UnityEngine;
using Enemy;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;

    [Header("UI")]
    [SerializeField] private HealthBar healthBar;

    public float currentHealth;

    // Read-only property
    public float MaxHealth => maxHealth;

    private EnemyAI enemyAI;
    private EnemySpawner spawner;

    private void Awake()
    {
        currentHealth = maxHealth;

        enemyAI = GetComponent<EnemyAI>();

        if (healthBar != null)
            healthBar.SetHealth(currentHealth, maxHealth);
    }

    // Called by EnemySpawner
    public void SetSpawner(EnemySpawner enemySpawner)
    {
        spawner = enemySpawner;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        Debug.Log($"{gameObject.name} HP : {currentHealth}");

        // Update Health Bar
        if (healthBar != null)
            healthBar.SetHealth(currentHealth, maxHealth);

        // Enemy Damage Animation
        if (enemyAI != null)
            enemyAI.PlayDamageAnimation();

        if (currentHealth <= 0)
        {
            // Notify Spawner
            if (spawner != null)
                spawner.EnemyKilled();

            if (enemyAI != null)
            {
                enemyAI.Die();
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        if (healthBar != null)
            healthBar.SetHealth(currentHealth, maxHealth);
    }

    public bool IsFullHealth()
    {
        return currentHealth >= maxHealth;
    }
}