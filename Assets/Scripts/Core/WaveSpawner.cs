using System;
using System.Collections;
using UnityEngine;
using PlanetDefense.Data;
using PlanetDefense.Enemy;
using PlanetDefense.Utils;

namespace PlanetDefense.Core
{
    public class WaveSpawner : MonoBehaviour
    {
        [Header("스폰 설정")]
        [SerializeField] private float spawnRadius = 12f;
        [SerializeField] private ObjectPool meteorPool;

        [Header("테스트 모드")]
        [SerializeField] private bool testMode = false;
        [SerializeField] private EnemyData testEnemyData;
        [SerializeField] private float testSpawnInterval = 1.5f;

        private int activeEnemyCount;
        private Coroutine currentWaveCoroutine;

        public int ActiveEnemyCount => activeEnemyCount;

        public event Action OnWaveCompleted;
        public event Action<EnemyBase> OnEnemyDied;

        public ObjectPool MeteorPool
        {
            get => meteorPool;
            set => meteorPool = value;
        }

        private void Start()
        {
            if (testMode && testEnemyData != null)
            {
                StartCoroutine(TestSpawnLoop());
            }
        }

        private IEnumerator TestSpawnLoop()
        {
            while (true)
            {
                SpawnMeteor(testEnemyData);
                yield return new WaitForSeconds(testSpawnInterval);
            }
        }

        public void SpawnWave(WaveData waveData)
        {
            if (currentWaveCoroutine != null)
                StopCoroutine(currentWaveCoroutine);

            currentWaveCoroutine = StartCoroutine(SpawnWaveCoroutine(waveData));
        }

        private IEnumerator SpawnWaveCoroutine(WaveData waveData)
        {
            yield return new WaitForSeconds(waveData.delayBeforeWave);

            for (int i = 0; i < waveData.count; i++)
            {
                SpawnMeteor(waveData.enemyType);
                yield return new WaitForSeconds(waveData.spawnInterval);
            }
        }

        private void SpawnMeteor(EnemyData data)
        {
            if (meteorPool == null) return;

            Vector3 spawnPos = GetRandomSpawnPosition();
            var obj = meteorPool.Get(spawnPos, Quaternion.identity);
            if (obj == null) return;

            float scale = UnityEngine.Random.Range(0.3f, 0.6f);
            obj.transform.localScale = Vector3.one * scale;

            var meteor = obj.GetComponent<Meteor>();
            if (meteor != null)
            {
                meteor.Initialize(data, meteorPool);
                meteor.OnDied += HandleEnemyDied;
                activeEnemyCount++;
            }
        }

        private Vector3 GetRandomSpawnPosition()
        {
            float angle = UnityEngine.Random.Range(0f, 360f) * Mathf.Deg2Rad;
            return new Vector3(
                Mathf.Cos(angle) * spawnRadius,
                Mathf.Sin(angle) * spawnRadius,
                0f
            );
        }

        private void HandleEnemyDied(EnemyBase enemy)
        {
            enemy.OnDied -= HandleEnemyDied;
            activeEnemyCount = Mathf.Max(0, activeEnemyCount - 1);
            OnEnemyDied?.Invoke(enemy);

            if (activeEnemyCount <= 0)
            {
                OnWaveCompleted?.Invoke();
            }
        }

        public void StopSpawning()
        {
            if (currentWaveCoroutine != null)
            {
                StopCoroutine(currentWaveCoroutine);
                currentWaveCoroutine = null;
            }
            StopAllCoroutines();
        }
    }
}