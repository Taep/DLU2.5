using UnityEngine;
using PlanetDefense.Utils;
using PlanetDefense.Enemy;

namespace PlanetDefense.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed = 15f;
        [SerializeField] private float lifeTime = 3f;

        private float damage;
        private Vector3 direction;
        private ObjectPool ownerPool;
        private float aliveTimer;

        public void Initialize(float damage, float speed, Vector3 direction, ObjectPool pool)
        {
            this.damage = damage;
            this.speed = speed;
            this.direction = direction.normalized;
            this.ownerPool = pool;
            aliveTimer = 0f;
        }

        private void Update()
        {
            transform.position += direction * speed * Time.deltaTime;

            aliveTimer += Time.deltaTime;
            if (aliveTimer >= lifeTime)
            {
                ReturnToPool();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            int hitLayer = other.gameObject.layer;

            // 함선 발사체 → 적 충돌
            if (hitLayer == LayerMask.NameToLayer("Enemy"))
            {
                var enemy = other.GetComponent<EnemyBase>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
                ReturnToPool();
                return;
            }

            // 보스 탄막 → 행성 충돌
            if (hitLayer == LayerMask.NameToLayer("Planet"))
            {
                var planet = other.GetComponent<Planet.Planet>();
                if (planet != null)
                {
                    planet.TakeDamage(damage);
                }
                ReturnToPool();
                return;
            }
        }

        private void ReturnToPool()
        {
            if (ownerPool != null)
            {
                ownerPool.Return(gameObject);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        private void OnEnable()
        {
            aliveTimer = 0f;
        }
    }
}