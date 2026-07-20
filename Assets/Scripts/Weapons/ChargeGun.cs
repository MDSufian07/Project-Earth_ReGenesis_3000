using UnityEngine;
using Cysharp.Threading.Tasks;
using Weapons;

public class ChargeGun : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private Bullet bulletPrefab;

    [Header("Effects")]
    [SerializeField] private GameObject muzzleEffect;

    [Header("Charge Settings")]
    [SerializeField] private float minimumCharge = 2f;
    [SerializeField] private float maximumCharge = 5f;

    [Header("Damage")]
    [SerializeField] private float minimumDamage = 20f;
    [SerializeField] private float maximumDamage = 100f;

    [Header("Audio")]
    [SerializeField] private AudioSource chargeAudioSource;
    [SerializeField] private AudioSource fireAudioSource;
    [SerializeField] private AudioClip fireSfx;

    private float chargeStartTime;
    private bool charging;
    
    public float MinimumCharge => minimumCharge;

    private void Start()
    {
        if (chargeAudioSource != null)
            chargeAudioSource.Stop();

        if (muzzleEffect != null)
            muzzleEffect.SetActive(false);
    }

    private void Update()
    {
        if (InputManager.Instance == null)
            return;

        if (InputManager.Instance.FirePressed)
        {
            if (GunCharge.Instance == null)
                return;
            
            if (!GunCharge.Instance.HasEnoughCharge(minimumCharge))
            {
                Debug.Log("Not enough Charge!");
                return;
            }

            charging = true;
            chargeStartTime = Time.time;

            if (chargeAudioSource != null &&
                !chargeAudioSource.isPlaying)
            {
                chargeAudioSource.Play();
            }

            StartMuzzleEffect().Forget();
        }

        if (InputManager.Instance.FireReleased)
        {
            Fire();
        }
    }
    
    public float CurrentChargePercent
    {
        get
        {
            if (!charging)
                return 0f;

            float holdTime = Time.time - chargeStartTime;

            return Mathf.Clamp01(
                (holdTime - minimumCharge) /
                (maximumCharge - minimumCharge));
        }
    }

    private async UniTaskVoid StartMuzzleEffect()
    {
        await UniTask.Delay(500);

        if (!charging)
            return;

        if (!InputManager.Instance.FireHeld)
            return;

        if (muzzleEffect != null)
            muzzleEffect.SetActive(true);

        await UniTask.WaitUntil(() => !InputManager.Instance.FireHeld);

        if (muzzleEffect != null)
            muzzleEffect.SetActive(false);
    }

    private void Fire()
    {
        if (!charging)
            return;

        charging = false;

        if (muzzleEffect != null)
            muzzleEffect.SetActive(false);

        if (chargeAudioSource != null &&
            chargeAudioSource.isPlaying)
        {
            chargeAudioSource.Stop();
        }

        float holdTime = Time.time - chargeStartTime;

        if (holdTime < minimumCharge)
            return;

        holdTime = Mathf.Clamp(holdTime, minimumCharge, maximumCharge);

        float chargeCost = holdTime;

        if (GunCharge.Instance == null)
            return;

        if (!GunCharge.Instance.HasEnoughCharge(chargeCost))
        {
            Debug.Log("Not enough Charge!");
            return;
        }

        GunCharge.Instance.Consume(chargeCost);

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

#if UNITY_EDITOR
        Debug.Log($"Hold Time: {holdTime:F2}s | Damage: {damage:F0} | Charge Used: {chargeCost:F2} | Remaining Charge: {GunCharge.Instance.CurrentCharge:F2}");
#endif
    }
}