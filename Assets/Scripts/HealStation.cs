using UnityEngine;

public class HealStation : MonoBehaviour
{
    [Header("Heal")]
    [SerializeField] private float healRate = 20f;

    [Header("References")]
    [SerializeField] private BoxCollider healTrigger;
    [SerializeField] private BoxCollider uiTrigger;

    [Header("UI")]
    [SerializeField] private GameObject pressEUI;

    private Transform player;
    private Health playerHealth;

    private void Start()
    {
        if (pressEUI != null)
            pressEUI.SetActive(false);

        GameObject p = GameObject.FindGameObjectWithTag("Player");

        if (p != null)
        {
            player = p.transform;
            playerHealth = p.GetComponent<Health>();
        }
    }

    private void Update()
    {
        if (player == null || playerHealth == null)
            return;

        bool inUIRange = IsInside(uiTrigger);
        bool inHealRange = IsInside(healTrigger);

        if (pressEUI != null)
            pressEUI.SetActive(inUIRange && !playerHealth.IsFullHealth());

        if (inHealRange &&
            !playerHealth.IsFullHealth() &&
            Input.GetKey(KeyCode.E))
        {
            playerHealth.Heal(healRate * Time.deltaTime);
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