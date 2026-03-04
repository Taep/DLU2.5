using UnityEngine;
using PlanetDefense.Data;

namespace PlanetDefense.Ship
{
    public class ShipController : MonoBehaviour
    {
        [SerializeField] private ShipData shipData;
        [SerializeField] private int level = 1;

        private ShipOrbit orbit;
        private ShipWeapon weapon;

        public ShipData Data => shipData;
        public int Level => level;

        public float CurrentDamage
        {
            get
            {
                if (shipData == null) return 0f;
                float multiplier = 1f + (level - 1) * shipData.damagePerLevel;
                return shipData.baseDamage * multiplier;
            }
        }

        public void Initialize(ShipData data, Transform orbitCenter, float startAngle)
        {
            shipData = data;

            orbit = GetComponent<ShipOrbit>();
            weapon = GetComponent<ShipWeapon>();

            if (orbit != null)
            {
                orbit.Initialize(orbitCenter, data.orbitRadius, data.orbitSpeed, startAngle);
            }

            if (weapon != null)
            {
                weapon.SetStats(data.baseDamage, data.fireRate, data.projectileSpeed);
            }

            level = 1;
        }

        public void LevelUp()
        {
            level++;
            ApplyLevelStats();
        }

        public void SetLevel(int newLevel)
        {
            level = Mathf.Max(1, newLevel);
            ApplyLevelStats();
        }

        private void ApplyLevelStats()
        {
            if (shipData == null || weapon == null) return;

            float damageMultiplier = 1f + (level - 1) * shipData.damagePerLevel;
            float fireRateMultiplier = 1f - (level - 1) * shipData.fireRatePerLevel;
            fireRateMultiplier = Mathf.Max(0.1f, fireRateMultiplier); // 최소 발사 간격

            weapon.SetStats(
                shipData.baseDamage * damageMultiplier,
                shipData.fireRate * fireRateMultiplier,
                shipData.projectileSpeed
            );
        }
    }
}