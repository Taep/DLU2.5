using UnityEngine;

namespace PlanetDefense.Data
{
    [CreateAssetMenu(fileName = "NewEnemyData", menuName = "PlanetDefense/Enemy Data")]
    public class EnemyData : ScriptableObject
    {
        [Header("기본 정보")]
        public string enemyName;
        public Sprite sprite;

        [Header("스탯")]
        public float hp = 30f;
        public float speed = 2f;
        public float damage = 100f;

        [Header("보스")]
        public bool isBoss;
        public float bossMultiplier = 1f;

        [Header("보상")]
        public int expReward = 10;
    }
}
