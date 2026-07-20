using UnityEngine;
using Weapons;

public class ChargeStation : MonoBehaviour
{
    [Header("Recharge")]
    [SerializeField] private float rechargeRate = 1f;

    [Header("UI (Optional)")]
    [SerializeField] private GameObject pressEUI;

    private bool playerInside;

    private void Start()
    {
        if (pressEUI != null)
            pressEUI.SetActive(false);
    }

    private void Update()
    {
        if (!playerInside)
            return;

        if (Input.GetKey(KeyCode.E))
        {
            GunCharge.Instance.Recharge(rechargeRate * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInside = true;

        if (pressEUI != null)
            pressEUI.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInside = false;

        if (pressEUI != null)
            pressEUI.SetActive(false);
    }
}