using UnityEngine;

namespace PlanetDefense.Data
{
    [CreateAssetMenu(fileName = "NewShipData", menuName = "PlanetDefense/Ship Data")]
    public class ShipData : ScriptableObject
    {
        [Header("기본 정보")]
        public string shipName;
        public Sprite icon;
        public Color shipColor = Color.white;

        [Header("전투")]
        public float baseDamage = 10f;
        public float fireRate = 0.5f;
        public float projectileSpeed = 15f;

        [Header("궤도")]
        public float orbitRadius = 3f;
        public float orbitSpeed = 30f;

        [Header("레벨업 계수")]
        [Tooltip("레벨당 데미지 증가 비율")]
        public float damagePerLevel = 0.15f;
        [Tooltip("레벨당 발사속도 증가 비율")]
        public float fireRatePerLevel = 0.05f;
    }
}
