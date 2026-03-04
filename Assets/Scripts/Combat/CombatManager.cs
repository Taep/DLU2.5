using UnityEngine;
using PlanetDefense.Utils;

namespace PlanetDefense.Combat
{
    public class CombatManager : MonoBehaviour
    {
        public static CombatManager Instance { get; private set; }

        [SerializeField] private ObjectPool damageNumberPool;
        [SerializeField] private ObjectPool explosionPool;

        public ObjectPool DamageNumberPool => damageNumberPool;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public void SpawnDamageNumber(float damage, Vector3 worldPos)
        {
            if (damageNumberPool == null) return;

            var obj = damageNumberPool.Get(worldPos, Quaternion.identity);
            if (obj == null) return;

            var dmgNum = obj.GetComponent<DamageNumber>();
            if (dmgNum != null)
            {
                dmgNum.Show(damage, worldPos, damageNumberPool);
            }
        }

        public void SpawnExplosion(Vector3 worldPos)
        {
            if (explosionPool == null) return;

            var obj = explosionPool.Get(worldPos, Quaternion.identity);
            if (obj == null) return;

            var ps = obj.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Clear();
                ps.Play();
            }

            // 자동 반환 코루틴
            StartCoroutine(ReturnExplosionDelayed(obj, 1f));
        }

        private System.Collections.IEnumerator ReturnExplosionDelayed(GameObject obj, float delay)
        {
            yield return new WaitForSeconds(delay);
            if (explosionPool != null && obj != null)
            {
                explosionPool.Return(obj);
            }
        }

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }
    }
}