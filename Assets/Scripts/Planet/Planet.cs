using System;
using UnityEngine;

namespace PlanetDefense.Planet
{
    public class Planet : MonoBehaviour
    {
        public static Planet Instance { get; private set; }

        [Header("체력")]
        [SerializeField] private float maxHP = 3000f;
        [SerializeField] private float currentHP;

        [Header("자전")]
        [SerializeField] private float rotationSpeed = 10f;
        [SerializeField] private Vector3 rotationAxis = new Vector3(0.3f, 1f, 0.2f);

        public float MaxHP => maxHP;
        public float CurrentHP => currentHP;
        public float HPRatio => maxHP > 0 ? currentHP / maxHP : 0f;
        public bool IsAlive => currentHP > 0;

        public event Action<float, float> OnHPChanged;       // currentHP, maxHP
        public event Action<float> OnDamaged;                  // damageAmount
        public event Action OnDestroyed;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void Start()
        {
            currentHP = maxHP;
            OnHPChanged?.Invoke(currentHP, maxHP);
        }

        private void Update()
        {
            transform.Rotate(rotationAxis.normalized, rotationSpeed * Time.deltaTime, Space.Self);
        }

        public void TakeDamage(float damage)
        {
            if (!IsAlive) return;

            damage = Mathf.Max(0, damage);
            currentHP = Mathf.Max(0, currentHP - damage);

            OnDamaged?.Invoke(damage);
            OnHPChanged?.Invoke(currentHP, maxHP);

            if (currentHP <= 0)
            {
                OnDestroyed?.Invoke();
            }
        }

        public void Heal(float amount)
        {
            if (!IsAlive) return;

            amount = Mathf.Max(0, amount);
            currentHP = Mathf.Min(maxHP, currentHP + amount);
            OnHPChanged?.Invoke(currentHP, maxHP);
        }

        public void ResetHP()
        {
            currentHP = maxHP;
            OnHPChanged?.Invoke(currentHP, maxHP);
        }

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }
    }
}
