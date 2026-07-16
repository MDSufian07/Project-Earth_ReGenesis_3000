using UnityEngine;

public class ChargeGun : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private Bullet bulletPrefab;

    [Header("Charge")]
    [SerializeField] private float minimumCharge = 1f;
    [SerializeField] private float maximumCharge = 3f;

    [Header("Damage")]
    [SerializeField] private float minimumDamage = 20f;
    [SerializeField] private float maximumDamage = 100f;

    private float chargeStartTime;
    private bool charging;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            charging = true;
            chargeStartTime = Time.time;
        }

        if (Input.GetMouseButtonUp(0))
        {
            Fire();
        }
    }

    private void Fire()
    {
        if (!charging)
            return;

        charging = false;

        float holdTime = Time.time - chargeStartTime;

        if (holdTime < minimumCharge)
            return;

        holdTime = Mathf.Min(holdTime, maximumCharge);

        float t = Mathf.InverseLerp(
            minimumCharge,
            maximumCharge,
            holdTime);

        float damage = Mathf.Lerp(
            minimumDamage,
            maximumDamage,
            t);

        Bullet bullet = Instantiate(
            bulletPrefab,
            firePoint.position,
            firePoint.rotation);

        bullet.Initialize(damage);
    }
}