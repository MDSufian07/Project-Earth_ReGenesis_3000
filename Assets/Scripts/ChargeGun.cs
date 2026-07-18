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
    [SerializeField] private AudioSource chargeAudioSource;
    [SerializeField] private AudioSource fireAudioSource;
    [SerializeField] private AudioClip fireSfx;

    private float chargeStartTime;
    private bool charging;

    private void Start()
    {
        if (chargeAudioSource != null)
            chargeAudioSource.Stop();
    }

    private void Update()
    {
        if (InputManager.Instance.FirePressed)
        {
            charging = true;
            chargeStartTime = Time.time;

            if (chargeAudioSource != null &&
                !chargeAudioSource.isPlaying)
            {
                chargeAudioSource.Play();
            }
        }

        if (InputManager.Instance.FireReleased)
        {
            if (chargeAudioSource != null &&
                chargeAudioSource.isPlaying)
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

        if (fireAudioSource != null && fireSfx != null)
        {
            fireAudioSource.PlayOneShot(fireSfx);
        }
    }
}