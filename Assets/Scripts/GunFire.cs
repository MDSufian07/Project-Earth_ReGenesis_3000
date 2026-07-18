using UnityEngine;
using Cysharp.Threading.Tasks;
using Reformation;

public class GunFire : MonoBehaviour
{
    [Header("Effects")]
    [SerializeField] private GameObject regenericEffect;
    [SerializeField] private GameObject muzzleEffect;

    [Header("Audio")]
    [SerializeField] private AudioSource reformationAudioSource; // Loop = ON

    [Header("Reformation")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private float repairRange = 5f;
    [SerializeField] private float repairRadius = 1.5f;
    [SerializeField] private LayerMask repairLayer;

    private bool leftTaskRunning = false;
    private bool rightTaskRunning = false;

    private void Start()
    {
        regenericEffect.SetActive(false);
        muzzleEffect.SetActive(false);

        if (reformationAudioSource != null)
        {
            reformationAudioSource.Stop();
        }
    }

    private void Update()
    {
        // Left Mouse
        if (Input.GetMouseButtonDown(0) && !leftTaskRunning)
        {
            HandleLeftClick().Forget();
        }

        // Right Mouse
        if (Input.GetMouseButtonDown(1) && !rightTaskRunning)
        {
            HandleRightClick().Forget();
        }

        // Repair while holding right mouse
        if (Input.GetMouseButton(1))
        {
            RepairObjects();
        }
    }

    private async UniTaskVoid HandleLeftClick()
    {
        leftTaskRunning = true;

        await UniTask.Delay(500);

        if (!Input.GetMouseButton(0))
        {
            leftTaskRunning = false;
            return;
        }

        muzzleEffect.SetActive(true);

        await UniTask.WaitUntil(() => !Input.GetMouseButton(0));

        muzzleEffect.SetActive(false);

        leftTaskRunning = false;
    }

    private async UniTaskVoid HandleRightClick()
    {
        rightTaskRunning = true;

        // Charge Time
        await UniTask.Delay(500);

        if (!Input.GetMouseButton(1))
        {
            rightTaskRunning = false;
            return;
        }

        // Enable Effect
        regenericEffect.SetActive(true);

        // Start Loop Sound
        if (reformationAudioSource != null && !reformationAudioSource.isPlaying)
        {
            reformationAudioSource.Play();
        }

        // Wait until mouse released
        await UniTask.WaitUntil(() => !Input.GetMouseButton(1));

        // Disable Effect
        regenericEffect.SetActive(false);

        // Stop Sound
        if (reformationAudioSource != null && reformationAudioSource.isPlaying)
        {
            reformationAudioSource.Stop();
        }

        rightTaskRunning = false;
    }

    private void RepairObjects()
    {
        RaycastHit[] hits = Physics.SphereCastAll(
            firePoint.position,
            repairRadius,
            firePoint.forward,
            repairRange,
            repairLayer);

        foreach (RaycastHit hit in hits)
        {
            ReformationObject obj = hit.collider.GetComponent<ReformationObject>();

            if (obj != null)
            {
                obj.Repair(Time.deltaTime);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (firePoint == null)
            return;

        Gizmos.color = Color.cyan;

        Vector3 endPoint = firePoint.position + firePoint.forward * repairRange;

        Gizmos.DrawWireSphere(firePoint.position, repairRadius);
        Gizmos.DrawWireSphere(endPoint, repairRadius);
        Gizmos.DrawLine(firePoint.position, endPoint);
    }
}