using UnityEngine;
using PlanetDefense.Data;
using PlanetDefense.Utils;

namespace PlanetDefense.Enemy
{
    public class Meteor : EnemyBase
    {
        [Header("운석 설정")]
        [SerializeField] private float rotationSpeed = 90f;

        private Vector3 randomRotationAxis;
        private Transform planetTransform;

        public override void Initialize(EnemyData data, ObjectPool pool)
        {
            base.Initialize(data, pool);

            randomRotationAxis = Random.onUnitSphere;
            rotationSpeed = Random.Range(60f, 180f);

            if (Planet.Planet.Instance != null)
                planetTransform = Planet.Planet.Instance.transform;
        }

        private void Update()
        {
            if (!IsAlive) return;

            // 행성 방향으로 이동
            if (planetTransform != null)
            {
                Vector3 direction = (planetTransform.position - transform.position).normalized;
                transform.position += direction * speed * Time.deltaTime;
            }

            // 랜덤 자전
            transform.Rotate(randomRotationAxis, rotationSpeed * Time.deltaTime, Space.Self);
        }

        private void OnTriggerEnter(Collider other)
        {
            // 행성과 충돌
            if (other.gameObject.layer == LayerMask.NameToLayer("Planet"))
            {
                var planet = other.GetComponent<Planet.Planet>();
                if (planet != null)
                {
                    planet.TakeDamage(damage);
                }
                Die();
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            randomRotationAxis = Random.onUnitSphere;

            if (Planet.Planet.Instance != null)
                planetTransform = Planet.Planet.Instance.transform;
        }
    }
}
