using TMPro;
using UnityEngine;

public class Warmup : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;

    private void Awake()
    {
        if (targetObject == null)
            return;

        TMP_Text[] texts = targetObject.GetComponentsInChildren<TMP_Text>(true);

        foreach (TMP_Text text in texts)
        {
            text.ForceMeshUpdate();
        }
    }
}