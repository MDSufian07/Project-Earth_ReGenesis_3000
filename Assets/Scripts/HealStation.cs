using UnityEngine;

public class HealStation : MonoBehaviour
{
    [Header("Heal")]
    [SerializeField] private float healRate = 20f;

    [Header("References")]
    [SerializeField] private Collider healTrigger;
    [SerializeField] private Collider uiTrigger;

    [Header("UI")]
    [SerializeField] private GameObject pressEUI;

    private bool inUIRange;
    private bool inHealRange;

    private Health playerHealth;

    private void Start()
    {
        if (pressEUI != null)
            pressEUI.SetActive(false);

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
            playerHealth = player.GetComponent<Health>();
    }

    private void Update()
    {
        if (playerHealth == null)
            return;

        // Show UI only when player is in UI range and not full health
        if (pressEUI != null)
        {
            pressEUI.SetActive(inUIRange && !playerHealth.IsFullHealth());
        }

        if (!inHealRange)
            return;

        if (playerHealth.IsFullHealth())
            return;

        if (Input.GetKey(KeyCode.E))
        {
            playerHealth.Heal(healRate * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (other.bounds.Intersects(uiTrigger.bounds))
            inUIRange = true;

        if (other.bounds.Intersects(healTrigger.bounds))
            inHealRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (other.bounds.Intersects(uiTrigger.bounds))
            inUIRange = false;

        if (other.bounds.Intersects(healTrigger.bounds))
            inHealRange = false;
    }
}