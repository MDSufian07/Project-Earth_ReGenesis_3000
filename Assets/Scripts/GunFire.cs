using UnityEngine;
using Cysharp.Threading.Tasks;
using Reformation;

public class GunFire : MonoBehaviour
{
    [Header("Effects")]
    [SerializeField] private GameObject regenericEffect;
    [SerializeField] private GameObject muzzleEffect;

    [Header("Audio")]
    [SerializeField] private AudioSource reformationAudioSource;

    [Header("Reformation")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private float repairRange = 5f;
    [SerializeField] private float repairRadius = 1.5f;
    [SerializeField] private LayerMask repairLayer;

    private bool leftTaskRunning;
    private bool rightTaskRunning;

    private void Start()
    {
        regenericEffect.SetActive(false);
        muzzleEffect.SetActive(false);

        if (reformationAudioSource != null)
            reformationAudioSource.Stop();
    }

    private void Update()
    {
        if (InputManager.Instance.FirePressed && !leftTaskRunning)
        {
            HandleLeftClick().Forget();
        }

        if (InputManager.Instance.RepairPressed && !rightTaskRunning)
        {
            HandleRightClick().Forget();
        }

        if (InputManager.Instance.RepairHeld)
        {
            RepairObjects();
        }
    }

    private async UniTaskVoid HandleLeftClick()
    {
        leftTaskRunning = true;

        await UniTask.Delay(500);

        if (!InputManager.Instance.FireHeld)
        {
            leftTaskRunning = false;
            return;
        }

        muzzleEffect.SetActive(true);

        await UniTask.WaitUntil(() => !InputManager.Instance.FireHeld);

        muzzleEffect.SetActive(false);

        leftTaskRunning = false;
    }

    private async UniTaskVoid HandleRightClick()
    {
        rightTaskRunning = true;

        await UniTask.Delay(500);

        if (!InputManager.Instance.RepairHeld)
        {
            rightTaskRunning = false;
            return;
        }

        regenericEffect.SetActive(true);

        if (reformationAudioSource != null &&
            !reformationAudioSource.isPlaying)
        {
            reformationAudioSource.Play();
        }

        await UniTask.WaitUntil(() => !InputManager.Instance.RepairHeld);

        regenericEffect.SetActive(false);

        if (reformationAudioSource != null &&
            reformationAudioSource.isPlaying)
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