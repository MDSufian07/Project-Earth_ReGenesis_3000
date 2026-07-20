using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Weapons;

namespace UI
{
    public class GunChargeUI : MonoBehaviour
    {
        [Header("Images")]
        [SerializeField] private Image chargeFill;
        [SerializeField] private Image fireChargeFill;

        [Header("Text")]
        [SerializeField] private TMP_Text chargeText;

        [Header("Charge Gun")]
        [SerializeField] private ChargeGun chargeGun;

        private void Update()
        {
            if (GunCharge.Instance == null)
                return;

            // Current Energy
            float current = GunCharge.Instance.CurrentCharge;
            float max = GunCharge.Instance.MaxCharge;

            chargeFill.fillAmount = current / max;

            // Fire Hold Preview
            if (chargeGun != null)
            {
                fireChargeFill.fillAmount = chargeGun.CurrentChargePercent;
            }

            // Text
            if (current < chargeGun.MinimumCharge)
            {
                chargeText.text = "NEED CHARGE";
            }
            else
            {
                chargeText.text = $"CHARGE : {Mathf.RoundToInt(current)} / {Mathf.RoundToInt(max)}";
            }
        }
    }
}