using UnityEngine;
using Cysharp.Threading.Tasks;
using Reformation;

public class GunFire : MonoBehaviour
{
    [Header("Effects")]
    [SerializeField] private GameObject regenericEffect;

    [Header("Audio")]
    [SerializeField] private AudioSource reformationAudioSource;

    [Header("Reformation")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private float repairRange = 5f;
    [SerializeField] private float repairRadius = 1.5f;
    [SerializeField] private LayerMask repairLayer;

    [Header("Optimization")]
    [SerializeField] private int maxHits = 20;

    private RaycastHit[] hitBuffer;
    private bool rightTaskRunning;

    private void Awake()
    {
        hitBuffer = new RaycastHit[maxHits];
    }

    private void Start()
    {
        if (regenericEffect != null)
            regenericEffect.SetActive(false);

        if (reformationAudioSource != null)
            reformationAudioSource.Stop();
    }

    private void Update()
    {
        if (InputManager.Instance == null)
            return;

        if (InputManager.Instance.RepairPressed && !rightTaskRunning)
        {
            HandleRightClick().Forget();
        }

        if (InputManager.Instance.RepairHeld)
        {
            RepairObjects();
        }
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

        if (regenericEffect != null)
            regenericEffect.SetActive(true);

        if (reformationAudioSource != null &&
            !reformationAudioSource.isPlaying)
        {
            reformationAudioSource.Play();
        }

        await UniTask.WaitUntil(() => !InputManager.Instance.RepairHeld);

        if (regenericEffect != null)
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
        if (firePoint == null)
            return;

        int hitCount = Physics.SphereCastNonAlloc(
            firePoint.position,
            repairRadius,
            firePoint.forward,
            hitBuffer,
            repairRange,
            repairLayer);

        for (int i = 0; i < hitCount; i++)
        {
            if (hitBuffer[i].collider == null)
                continue;

            ReformationObject obj =
                hitBuffer[i].collider.GetComponent<ReformationObject>();

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