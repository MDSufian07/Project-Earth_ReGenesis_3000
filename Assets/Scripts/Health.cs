using UnityEngine;
using Enemy;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;

    public float currentHealth;
    private EnemyAI enemyAI;

    private void Awake()
    {
        currentHealth = maxHealth;
        enemyAI = GetComponent<EnemyAI>();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        Debug.Log($"{gameObject.name} HP : {currentHealth}");

        // Enemy হলে Damage Animation
        if (enemyAI != null)
            enemyAI.PlayDamageAnimation();

        if (currentHealth <= 0)
        {
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
}