using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 25f;
    [SerializeField] private float lifeTime = 5f;
    [SerializeField] private GameObject impactEffect;

    private float damage;

    public void Initialize(float bulletDamage)
    {
        damage = bulletDamage;

        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        Health health = other.GetComponent<Health>();

        if (health != null)
        {
            health.TakeDamage(damage);
        }

        if (impactEffect != null)
        {
            Instantiate(
                impactEffect,
                transform.position,
                transform.rotation * Quaternion.Euler(90f, 0f, 0f));
        }

        Destroy(gameObject);
    }
}