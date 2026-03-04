using System;
using UnityEngine;
using PlanetDefense.Data;
using PlanetDefense.Utils;
using PlanetDefense.Combat;

namespace PlanetDefense.Enemy
{
    public class EnemyBase : MonoBehaviour
    {
        [Header("스탯")]
        [SerializeField] protected float maxHP = 30f;
        [SerializeField] protected float currentHP;
        [SerializeField] protected float speed = 2f;
        [SerializeField] protected float damage = 100f;

        [Header("보상")]
        [SerializeField] protected int expReward = 10;

        protected ObjectPool ownerPool;

        public float MaxHP => maxHP;
        public float CurrentHP => currentHP;
        public float HPRatio => maxHP > 0 ? currentHP / maxHP : 0f;
        public bool IsAlive => currentHP > 0;
        public int ExpReward => expReward;

        public event Action<EnemyBase> OnDied;
        public event Action<float, float> OnHPChanged;

        public virtual void Initialize(EnemyData data, ObjectPool pool)
        {
            maxHP = data.hp;
            currentHP = data.hp;
            speed = data.speed;
            damage = data.damage;
            expReward = data.expReward;
            ownerPool = pool;

            OnHPChanged?.Invoke(currentHP, maxHP);
        }

        public virtual void TakeDamage(float damageAmount)
        {
            if (!IsAlive) return;

            damageAmount = Mathf.Max(0, damageAmount);
            currentHP = Mathf.Max(0, currentHP - damageAmount);

            OnHPChanged?.Invoke(currentHP, maxHP);
            SpawnDamageNumber(damageAmount);

            if (currentHP <= 0)
            {
                Die();
            }
        }

        protected virtual void SpawnDamageNumber(float damageAmount)
        {
            if (CombatManager.Instance != null)
            {
                Vector3 offset = new Vector3(
                    UnityEngine.Random.Range(-0.3f, 0.3f),
                    UnityEngine.Random.Range(0.2f, 0.5f),
                    0f
                );
                CombatManager.Instance.SpawnDamageNumber(damageAmount, transform.position + offset);
            }
        }

        protected virtual void Die()
        {
            SpawnExplosion();
            InvokeOnDied();
            ReturnToPool();
        }

        protected virtual void SpawnExplosion()
        {
            if (CombatManager.Instance != null)
            {
                CombatManager.Instance.SpawnExplosion(transform.position);
            }
        }

        protected void InvokeOnDied()
        {
            OnDied?.Invoke(this);
        }

        protected void ReturnToPool()
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

        protected virtual void OnEnable()
        {
            // Reset for pool reuse
        }
    }
}