using UnityEngine;

namespace Weapons
{
    public class GunCharge : MonoBehaviour
    {
        public static GunCharge Instance;

        [Header("Charge")]
        [SerializeField] private float maxCharge = 100f;
        [SerializeField] private float currentCharge = 100f;

        [Header("Recharge")]
        [SerializeField] private float rechargeRate = 1f;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        public float CurrentCharge => currentCharge;
        public float MaxCharge => maxCharge;

        public bool HasEnoughCharge(float amount)
        {
            return currentCharge >= amount;
        }

        public void Consume(float amount)
        {
            currentCharge -= amount;
            currentCharge = Mathf.Clamp(currentCharge, 0f, maxCharge);
        }

        public void Recharge(float deltaTime)
        {
            currentCharge += rechargeRate * deltaTime;
            currentCharge = Mathf.Clamp(currentCharge, 0f, maxCharge);
        }

        public bool IsFull()
        {
            return currentCharge >= maxCharge;
        }
    }
}