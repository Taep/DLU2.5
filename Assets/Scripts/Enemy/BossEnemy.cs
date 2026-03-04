using System;
using UnityEngine;
using PlanetDefense.Data;
using PlanetDefense.Utils;
using PlanetDefense.Combat;

namespace PlanetDefense.Enemy
{
    public class BossEnemy : EnemyBase
    {
        [Header("보스 설정")]
        [SerializeField] private float horizontalSpeed = 1.5f;
        [SerializeField] private float horizontalRange = 3f;
        [SerializeField] private float entrySpeed = 3f;
        [SerializeField] private Vector3 targetPosition = new Vector3(0, 6, 0);

        [Header("탄막")]
        [SerializeField] private float bulletInterval = 1.5f;
        [SerializeField] private float bulletSpeed = 5f;
        [SerializeField] private float bulletDamage = 50f;
        [SerializeField] private int bulletsPerShot = 3;
        [SerializeField] private float spreadAngle = 30f;

        [Header("참조")]
        [SerializeField] private ObjectPool bulletPool;

        private float bulletTimer;
        private float moveTimer;
        private bool hasArrived;
        private Vector3 arrivalPosition;

        public event Action<float, float> OnBossHPChanged;

        public ObjectPool BulletPool
        {
            set => bulletPool = value;
        }

        public override void Initialize(EnemyData data, ObjectPool pool)
        {
            base.Initialize(data, pool);

            if (data.isBoss)
            {
                maxHP *= data.bossMultiplier;
                currentHP = maxHP;
            }

            hasArrived = false;
            bulletTimer = 0f;
            moveTimer = 0f;
            arrivalPosition = targetPosition;
        }

        private void Update()
        {
            if (!IsAlive) return;

            if (!hasArrived)
            {
                transform.position = Vector3.MoveTowards(transform.position, arrivalPosition, entrySpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, arrivalPosition) < 0.1f)
                    hasArrived = true;
                return;
            }

            moveTimer += Time.deltaTime;
            float xOffset = Mathf.Sin(moveTimer * horizontalSpeed) * horizontalRange;
            transform.position = new Vector3(arrivalPosition.x + xOffset, arrivalPosition.y, arrivalPosition.z);

            bulletTimer += Time.deltaTime;
            if (bulletTimer >= bulletInterval)
            {
                FireBullets();
                bulletTimer = 0f;
            }
        }

        private void FireBullets()
        {
            if (bulletPool == null) return;

            var planetPos = Planet.Planet.Instance != null
                ? Planet.Planet.Instance.transform.position
                : Vector3.zero;

            Vector3 baseDirection = (planetPos - transform.position).normalized;

            if (bulletsPerShot == 1)
            {
                SpawnBullet(baseDirection);
                return;
            }

            float startAngle = -spreadAngle / 2f;
            float angleStep = spreadAngle / (bulletsPerShot - 1);

            for (int i = 0; i < bulletsPerShot; i++)
            {
                float angle = startAngle + angleStep * i;
                Vector3 dir = Quaternion.Euler(0, 0, angle) * baseDirection;
                SpawnBullet(dir);
            }
        }

        private void SpawnBullet(Vector3 direction)
        {
            var obj = bulletPool.Get(transform.position, Quaternion.identity);
            if (obj == null) return;

            var proj = obj.GetComponent<Projectile>();
            if (proj != null)
            {
                proj.Initialize(bulletDamage, bulletSpeed, direction, bulletPool);
            }
        }

        public override void TakeDamage(float damageAmount)
        {
            base.TakeDamage(damageAmount);
            OnBossHPChanged?.Invoke(currentHP, maxHP);
        }

        protected override void Die()
        {
            SpawnExplosion();
            InvokeOnDied();
            ReturnToPool();
        }
    }
}