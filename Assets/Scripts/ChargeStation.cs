using UnityEngine;
using Weapons;

public class ChargeStation : MonoBehaviour
{
    [Header("Recharge")]
    [SerializeField] private float rechargeRate = 5f;

    [Header("References")]
    [SerializeField] private BoxCollider rechargeTrigger;
    [SerializeField] private BoxCollider uiTrigger;

    [Header("UI")]
    [SerializeField] private GameObject pressEUI;

    private Transform player;

    private void Start()
    {
        if (pressEUI != null)
            pressEUI.SetActive(false);

        GameObject p = GameObject.FindGameObjectWithTag("Player");

        if (p != null)
            player = p.transform;
    }

    private void Update()
    {
        if (player == null)
            return;

        bool inUIRange = IsInside(uiTrigger);
        bool inRechargeRange = IsInside(rechargeTrigger);

        if (pressEUI != null)
            pressEUI.SetActive(inUIRange);

        if (inRechargeRange && Input.GetKey(KeyCode.E))
        {
            GunCharge.Instance.Recharge(rechargeRate * Time.deltaTime);
        }
    }

    private bool IsInside(BoxCollider box)
    {
        if (box == null)
            return false;

        Vector3 worldCenter = box.transform.TransformPoint(box.center);
        Vector3 worldHalfSize = Vector3.Scale(box.size * 0.5f, box.transform.lossyScale);

        Collider[] hits = Physics.OverlapBox(
            worldCenter,
            worldHalfSize,
            box.transform.rotation
        );

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Player"))
                return true;
        }

        return false;
    }
}