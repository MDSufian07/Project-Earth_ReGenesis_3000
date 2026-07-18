using UnityEngine;

public class ChargeGun : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private Bullet bulletPrefab;

    [Header("Charge Settings")]
    [SerializeField] private float minimumCharge = 1f;
    [SerializeField] private float maximumCharge = 3f;

    [Header("Damage")]
    [SerializeField] private float minimumDamage = 20f;
    [SerializeField] private float maximumDamage = 100f;

    [Header("Audio")]
    [SerializeField] private AudioSource chargeAudioSource; // Loop = ON
    [SerializeField] private AudioSource fireAudioSource;   // Loop = OFF
    [SerializeField] private AudioClip fireSfx;

    private float chargeStartTime;
    private bool charging;

    private void Start()
    {
        if (chargeAudioSource != null)
        {
            chargeAudioSource.Stop();
        }
    }

    private void Update()
    {
        // Start Charging
        if (Input.GetMouseButtonDown(0))
        {
            charging = true;
            chargeStartTime = Time.time;

            if (chargeAudioSource != null && !chargeAudioSource.isPlaying)
            {
                chargeAudioSource.Play();
            }
        }

        // Release Fire
        if (Input.GetMouseButtonUp(0))
        {
            if (chargeAudioSource != null && chargeAudioSource.isPlaying)
            {
                chargeAudioSource.Stop();
            }

            Fire();
        }
    }

    private void Fire()
    {
        if (!charging)
            return;

        charging = false;

        float holdTime = Time.time - chargeStartTime;

        // Not enough charge
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

        // Spawn Bullet
        Bullet bullet = Instantiate(
            bulletPrefab,
            firePoint.position,
            firePoint.rotation);

        bullet.Initialize(damage);

        // Fire Sound
        if (fireAudioSource != null && fireSfx != null)
        {
            fireAudioSource.PlayOneShot(fireSfx);
        }
    }
}