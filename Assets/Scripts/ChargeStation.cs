using UnityEngine;
using Weapons;

public class ChargeStation : MonoBehaviour
{
    [Header("Recharge")]
    [SerializeField] private float rechargeRate = 1f;

    [Header("References")]
    [SerializeField] private Collider rechargeTrigger;
    [SerializeField] private Collider uiTrigger;

    [Header("UI")]
    [SerializeField] private GameObject pressEUI;

    private bool inUIRange;
    private bool inRechargeRange;

    private void Start()
    {
        if (pressEUI != null)
            pressEUI.SetActive(false);
    }

    private void Update()
    {
        if (pressEUI != null)
            pressEUI.SetActive(inUIRange);

        if (inRechargeRange && Input.GetKey(KeyCode.E))
        {
            GunCharge.Instance.Recharge(rechargeRate * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (other == null)
            return;

        if (other.bounds.Intersects(uiTrigger.bounds))
            inUIRange = true;

        if (other.bounds.Intersects(rechargeTrigger.bounds))
            inRechargeRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (other.bounds.Intersects(uiTrigger.bounds))
            inUIRange = false;

        if (other.bounds.Intersects(rechargeTrigger.bounds))
            inRechargeRange = false;
    }
}