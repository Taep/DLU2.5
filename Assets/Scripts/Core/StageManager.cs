using System;
using UnityEngine;
using PlanetDefense.Data;
using PlanetDefense.Enemy;
using PlanetDefense.Ship;
using PlanetDefense.Utils;

namespace PlanetDefense.Core
{
    public class StageManager : MonoBehaviour
    {
        public static StageManager Instance { get; private set; }

        [Header("스테이지 데이터")]
        [SerializeField] private StageData currentStageData;

        [Header("참조")]
        [SerializeField] private WaveSpawner waveSpawner;

        [Header("보스")]
        [SerializeField] private ObjectPool bossPool;
        [SerializeField] private ObjectPool bossBulletPool;

        [Header("레벨업")]
        [SerializeField] private int expPerLevel = 50;

        private int currentWaveIndex;
        private int totalExp;
        private int currentLevel = 1;
        private float stageTimer;
        private bool stageActive;
        private bool bossPhase;
        private BossEnemy activeBoss;

        public StageData CurrentStageData => currentStageData;
        public int CurrentWaveIndex => currentWaveIndex;
        public int TotalWaves => currentStageData != null ? currentStageData.waves.Length : 0;
        public int CurrentLevel => currentLevel;
        public int TotalExp => totalExp;
        public float StageTimer => stageTimer;
        public bool IsStageActive => stageActive;
        public bool IsBossPhase => bossPhase;
        public BossEnemy ActiveBoss => activeBoss;

        public event Action<int> OnLevelUp;
        public event Action<int, int> OnWaveChanged;
        public event Action<BossEnemy> OnBossPhaseStarted;
        public event Action OnAllWavesCompleted;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public void StartStage(StageData data)
        {
            currentStageData = data;
            currentWaveIndex = 0;
            totalExp = 0;
            currentLevel = 1;
            stageTimer = 0f;
            stageActive = true;
            bossPhase = false;

            if (waveSpawner != null)
            {
                waveSpawner.StopSpawning();
                waveSpawner.OnWaveCompleted += HandleWaveCompleted;
                waveSpawner.OnEnemyDied += HandleEnemyDied;
            }

            StartNextWave();
        }

        private void Update()
        {
            if (!stageActive) return;
            stageTimer += Time.deltaTime;
        }

        private void StartNextWave()
        {
            if (currentStageData == null || waveSpawner == null) return;

            if (currentWaveIndex < currentStageData.waves.Length)
            {
                OnWaveChanged?.Invoke(currentWaveIndex + 1, currentStageData.waves.Length);
                waveSpawner.SpawnWave(currentStageData.waves[currentWaveIndex]);
            }
            else
            {
                OnAllWavesCompleted?.Invoke();
                StartBossPhase();
            }
        }

        private void HandleWaveCompleted()
        {
            currentWaveIndex++;
            StartNextWave();
        }

        private void HandleEnemyDied(EnemyBase enemy)
        {
            AddExp(enemy.ExpReward);
        }

        public void AddExp(int exp)
        {
            totalExp += exp;
            int newLevel = (totalExp / expPerLevel) + 1;

            if (newLevel > currentLevel)
            {
                currentLevel = newLevel;
                LevelUpAllShips();
                OnLevelUp?.Invoke(currentLevel);
            }
        }

        private void LevelUpAllShips()
        {
            var ships = FindObjectsOfType<ShipController>();
            foreach (var ship in ships)
            {
                ship.SetLevel(currentLevel);
            }
        }

        private void StartBossPhase()
        {
            if (currentStageData.bossData == null)
            {
                if (GameManager.Instance != null)
                    GameManager.Instance.StageClear();
                return;
            }

            bossPhase = true;
            SpawnBoss();
        }

        private void SpawnBoss()
        {
            if (bossPool == null)
            {
                Debug.LogError("[StageManager] bossPool이 할당되지 않았습니다.");
                return;
            }

            Vector3 spawnPos = new Vector3(0f, 12f, 0f);
            var obj = bossPool.Get(spawnPos, Quaternion.identity);
            if (obj == null) return;

            activeBoss = obj.GetComponent<BossEnemy>();
            if (activeBoss == null) return;

            activeBoss.Initialize(currentStageData.bossData, bossPool);

            if (bossBulletPool != null)
            {
                activeBoss.BulletPool = bossBulletPool;
            }

            activeBoss.OnDied += HandleBossDied;
            OnBossPhaseStarted?.Invoke(activeBoss);
        }

        private void HandleBossDied(EnemyBase boss)
        {
            boss.OnDied -= HandleBossDied;
            activeBoss = null;
            bossPhase = false;

            if (GameManager.Instance != null)
                GameManager.Instance.StageClear();
        }

        public void StopStage()
        {
            stageActive = false;

            if (waveSpawner != null)
            {
                waveSpawner.StopSpawning();
                waveSpawner.OnWaveCompleted -= HandleWaveCompleted;
                waveSpawner.OnEnemyDied -= HandleEnemyDied;
            }

            if (activeBoss != null)
            {
                activeBoss.OnDied -= HandleBossDied;
                activeBoss = null;
            }

            ReturnAllPooledObjects();
        }

        private void ReturnAllPooledObjects()
        {
            if (waveSpawner != null && waveSpawner.MeteorPool != null)
                waveSpawner.MeteorPool.ReturnAll();

            if (bossPool != null)
                bossPool.ReturnAll();

            if (bossBulletPool != null)
                bossBulletPool.ReturnAll();
        }

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }
    }
}