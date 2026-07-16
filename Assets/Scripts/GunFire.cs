using UnityEngine;
using Cysharp.Threading.Tasks;

public class GunFire : MonoBehaviour
{
    [SerializeField] private GameObject regenericEffect;
    [SerializeField] private GameObject muzzleEffect;

    private bool holdTaskRunning = false;

    private void Start()
    {
        regenericEffect.SetActive(false);
    }

    private void Update()
    {
        // Left Click
        if (Input.GetMouseButtonDown(0))
        {
            muzzleEffect.SetActive(true);

            // যদি Particle System হয় তাহলে Play() ব্যবহার করুন
            // muzzleEffect.GetComponent<ParticleSystem>()?.Play();
        }

        // Right Click
        if (Input.GetMouseButtonDown(1) && !holdTaskRunning)
        {
            HandleRightClick().Forget();
        }
    }

    private async UniTaskVoid HandleRightClick()
    {
        holdTaskRunning = true;

        // Wait 0.5 second
        await UniTask.Delay(500);

        // Released before 0.5 sec
        if (!Input.GetMouseButton(1))
        {
            holdTaskRunning = false;
            return;
        }

        // Enable effect
        regenericEffect.SetActive(true);

        // Wait until released
        await UniTask.WaitUntil(() => !Input.GetMouseButton(1));

        // Disable immediately
        regenericEffect.SetActive(false);

        holdTaskRunning = false;
    }
}