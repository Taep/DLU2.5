using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlanetDefense.Ship;
using PlanetDefense.Data;

namespace PlanetDefense.UI
{
    public class ShipSlotUI : MonoBehaviour
    {
        [Header("UI 요소")]
        [SerializeField] private Image shipIcon;
        [SerializeField] private Image shipColorIndicator;
        [SerializeField] private TextMeshProUGUI shipNameText;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI damageText;

        [Header("참조")]
        [SerializeField] private ShipController targetShip;

        private void Start()
        {
            if (targetShip != null)
            {
                InitializeSlot();
            }
        }

        public void SetTargetShip(ShipController ship)
        {
            targetShip = ship;
            InitializeSlot();
        }

        private void InitializeSlot()
        {
            if (targetShip == null) return;

            ShipData data = targetShip.Data;
            if (data == null) return;

            if (shipNameText != null)
                shipNameText.text = data.shipName;

            if (shipIcon != null && data.icon != null)
                shipIcon.sprite = data.icon;

            if (shipColorIndicator != null)
                shipColorIndicator.color = data.shipColor;

            UpdateStats();
        }

        private void Update()
        {
            if (targetShip == null) return;
            UpdateStats();
        }

        private void UpdateStats()
        {
            if (targetShip == null) return;

            if (levelText != null)
                levelText.text = $"Lv.{targetShip.Level}";

            if (damageText != null)
            {
                float dmg = targetShip.CurrentDamage;
                damageText.text = $"DMG: {dmg:F0}";
            }
        }
    }
}