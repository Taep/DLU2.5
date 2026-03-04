using UnityEngine;
using PlanetDefense.Utils;
using PlanetDefense.Combat;

namespace PlanetDefense.Ship
{
    public class ShipWeapon : MonoBehaviour
    {
        [Header("전투 설정")]
        [SerializeField] private float damage = 10f;
        [SerializeField] private float fireRate = 0.5f;
        [SerializeField] private float projectileSpeed = 15f;
        [SerializeField] private float detectionRange = 20f;

        [Header("참조")]
        [SerializeField] private ObjectPool projectilePool;

        private float fireTimer;
        private Transform currentTarget;
        private int enemyLayerMask;

        public float Damage
        {
            get => damage;
            set => damage = value;
        }

        public float FireRate
        {
            get => fireRate;
            set => fireRate = value;
        }

        public float ProjectileSpeed
        {
            get => projectileSpeed;
            set => projectileSpeed = value;
        }

        public ObjectPool ProjectilePool
        {
            set => projectilePool = value;
        }

        private void Start()
        {
            enemyLayerMask = 1 << LayerMask.NameToLayer("Enemy");
        }

        private void Update()
        {
            fireTimer += Time.deltaTime;

            currentTarget = FindClosestTarget();

            if (currentTarget != null)
            {
                LookAtTarget(currentTarget);

                if (fireTimer >= fireRate)
                {
                    Fire(currentTarget);
                    fireTimer = 0f;
                }
            }
        }

        private Transform FindClosestTarget()
        {
            var colliders = Physics.OverlapSphere(transform.position, detectionRange, enemyLayerMask);
            if (colliders.Length == 0) return null;

            Transform closest = null;
            float closestDist = float.MaxValue;

            for (int i = 0; i < colliders.Length; i++)
            {
                float dist = Vector3.SqrMagnitude(colliders[i].transform.position - transform.position);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    closest = colliders[i].transform;
                }
            }

            return closest;
        }

        private void LookAtTarget(Transform target)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
            }
        }

        private void Fire(Transform target)
        {
            if (projectilePool == null) return;

            var projObj = projectilePool.Get(transform.position, Quaternion.identity);
            if (projObj == null) return;

            Vector3 direction = (target.position - transform.position).normalized;

            var projectile = projObj.GetComponent<Projectile>();
            if (projectile != null)
            {
                projectile.Initialize(damage, projectileSpeed, direction, projectilePool);
            }
        }

        public void SetStats(float newDamage, float newFireRate, float newProjectileSpeed)
        {
            damage = newDamage;
            fireRate = newFireRate;
            projectileSpeed = newProjectileSpeed;
        }
    }
}
