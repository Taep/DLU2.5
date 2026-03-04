using UnityEngine;

namespace PlanetDefense.Data
{
    [CreateAssetMenu(fileName = "NewStageData", menuName = "PlanetDefense/Stage Data")]
    public class StageData : ScriptableObject
    {
        [Header("스테이지 정보")]
        public string stageName = "일반 스테이지 1";
        public float timeLimit = 180f;

        [Header("웨이브")]
        public WaveData[] waves;

        [Header("보스")]
        public EnemyData bossData;
    }

    [System.Serializable]
    public class WaveData
    {
        public EnemyData enemyType;
        public int count = 5;
        public float spawnInterval = 1.5f;
        public float delayBeforeWave = 2f;
    }
}
