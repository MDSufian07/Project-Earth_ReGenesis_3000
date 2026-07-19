using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Header("Settings")]
    public float interactRange = 1f;
    public LayerMask collectibleLayer;

    private CollectableItem currentItem;

    private void Update()
    {
        FindCollectable();

        if (currentItem != null && Input.GetKeyDown(KeyCode.E))
        {
            currentItem.Collect();
            currentItem = null;
        }
    }

    void FindCollectable()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, interactRange, collectibleLayer);

        CollectableItem nearest = null;

        if (hits.Length > 0)
        {
            nearest = hits[0].GetComponent<CollectableItem>();
        }

        if (nearest != currentItem)
        {
            if (currentItem != null)
                currentItem.ShowPrompt(false);

            currentItem = nearest;

            if (currentItem != null)
                currentItem.ShowPrompt(true);
        }

        if (hits.Length == 0 && currentItem != null)
        {
            currentItem.ShowPrompt(false);
            currentItem = null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}