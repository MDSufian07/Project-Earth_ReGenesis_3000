using UnityEngine;

namespace Reformation
{
    public class ReformationObject : MonoBehaviour
    {
        public enum RepairMode
        {
            PartByPart,
            SwapObject
        }

        [Header("Mode")]
        [SerializeField] private RepairMode repairMode = RepairMode.PartByPart;

        [Header("Part By Part")]
        [SerializeField] private GameObject[] repairParts;

        [Header("Swap Object")]
        [SerializeField] private GameObject brokenObject;
        [SerializeField] private GameObject repairedObject;

        [Header("Settings")]
        [SerializeField] private float repairTimePerPart = 0.3f;

        [Header("Complete Effect")]
        [SerializeField] private GameObject completeEffect;
        [SerializeField] private Transform effectPoint;

        private float timer;
        private int currentPart;
        private bool repaired;

        private void Start()
        {
            if (repairMode == RepairMode.PartByPart)
            {
                foreach (var part in repairParts)
                {
                    if (part != null)
                        part.SetActive(false);
                }
            }
            else
            {
                if (brokenObject != null)
                    brokenObject.SetActive(true);

                if (repairedObject != null)
                    repairedObject.SetActive(false);
            }
        }

        public void Repair(float deltaTime)
        {
            if (repaired)
                return;

            timer += deltaTime;

            if (timer < repairTimePerPart)
                return;

            timer = 0f;

            switch (repairMode)
            {
                case RepairMode.PartByPart:
                    RepairNextPart();
                    break;

                case RepairMode.SwapObject:
                    CompleteSwap();
                    break;
            }
        }

        private void RepairNextPart()
        {
            if (currentPart >= repairParts.Length)
                return;

            repairParts[currentPart].SetActive(true);
            currentPart++;

            if (currentPart >= repairParts.Length)
            {
                FinishRepair();
            }
        }

        private void CompleteSwap()
        {
            if (brokenObject != null)
                brokenObject.SetActive(false);

            if (repairedObject != null)
                repairedObject.SetActive(true);

            FinishRepair();
        }

        private void FinishRepair()
        {
            repaired = true;

            if (completeEffect != null)
            {
                Instantiate(
                    completeEffect,
                    effectPoint != null ? effectPoint.position : transform.position,
                    Quaternion.identity);
            }
        }
    }
}