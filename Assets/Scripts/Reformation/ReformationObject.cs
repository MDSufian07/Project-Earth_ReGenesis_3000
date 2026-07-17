using UnityEngine;

namespace Reformation
{
    public class ReformationObject : MonoBehaviour
    {
        [SerializeField] private GameObject[] repairParts;
        [SerializeField] private float repairTimePerPart = 0.3f;
        [SerializeField] private GameObject completeEffect;
        [SerializeField] private Transform effectPoint;

        private float timer;
        private int currentPart;
        private bool repaired;

        private void Start()
        {
            foreach (var part in repairParts)
                part.SetActive(false);
        }

        public void Repair(float deltaTime)
        {
            if (repaired)
                return;

            timer += deltaTime;

            if (timer >= repairTimePerPart)
            {
                timer = 0;

                repairParts[currentPart].SetActive(true);
                currentPart++;

                if (currentPart >= repairParts.Length)
                {
                    repaired = true;

                    if (completeEffect != null)
                        Instantiate(completeEffect, effectPoint.position, Quaternion.identity);
                }
            }
        }
    }
}