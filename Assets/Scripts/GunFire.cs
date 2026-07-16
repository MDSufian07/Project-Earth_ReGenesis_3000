using UnityEngine;
using Cysharp.Threading.Tasks;

public class GunFire : MonoBehaviour
{
    [SerializeField] private GameObject regenericEffect;
    [SerializeField] private GameObject muzzleEffect;

    private bool leftTaskRunning = false;
    private bool rightTaskRunning = false;

    private void Start()
    {
        regenericEffect.SetActive(false);
        muzzleEffect.SetActive(false);
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
    }

    private async UniTaskVoid HandleLeftClick()
    {
        leftTaskRunning = true;

        // Wait 0.5 sec
        await UniTask.Delay(500);

        // Released before 0.5 sec
        if (!Input.GetMouseButton(0))
        {
            leftTaskRunning = false;
            return;
        }

        // Enable effect
        muzzleEffect.SetActive(true);

        // Wait until released
        await UniTask.WaitUntil(() => !Input.GetMouseButton(0));

        // Disable immediately
        muzzleEffect.SetActive(false);

        leftTaskRunning = false;
    }

    private async UniTaskVoid HandleRightClick()
    {
        rightTaskRunning = true;

        // Wait 0.5 sec
        await UniTask.Delay(500);

        // Released before 0.5 sec
        if (!Input.GetMouseButton(1))
        {
            rightTaskRunning = false;
            return;
        }

        // Enable effect
        regenericEffect.SetActive(true);

        // Wait until released
        await UniTask.WaitUntil(() => !Input.GetMouseButton(1));

        // Disable immediately
        regenericEffect.SetActive(false);

        rightTaskRunning = false;
    }
}