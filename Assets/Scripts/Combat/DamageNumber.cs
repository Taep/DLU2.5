using UnityEngine;
using TMPro;
using PlanetDefense.Utils;

namespace PlanetDefense.Combat
{
    public class DamageNumber : MonoBehaviour
    {
        [Header("설정")]
        [SerializeField] private float floatSpeed = 2f;
        [SerializeField] private float duration = 0.8f;
        [SerializeField] private float scaleStart = 0.8f;
        [SerializeField] private float scaleEnd = 0.4f;

        private TextMeshPro textMesh;
        private ObjectPool ownerPool;
        private float timer;
        private Color startColor;
        private Vector3 startPos;

        private void Awake()
        {
            textMesh = GetComponent<TextMeshPro>();
            if (textMesh == null)
                textMesh = GetComponentInChildren<TextMeshPro>();
        }

        public void Show(float damage, Vector3 worldPos, ObjectPool pool)
        {
            ownerPool = pool;
            timer = 0f;
            startPos = worldPos;
            transform.position = worldPos;

            if (textMesh != null)
            {
                textMesh.text = $"-{Mathf.RoundToInt(damage)}";
                textMesh.color = GetDamageColor(damage);
                startColor = textMesh.color;
            }

            transform.localScale = Vector3.one * scaleStart;
        }

        private Color GetDamageColor(float damage)
        {
            if (damage >= 50f) return new Color(1f, 0.3f, 0.2f); // red for high
            if (damage >= 20f) return new Color(1f, 0.8f, 0.2f); // yellow for medium
            return Color.white; // white for low
        }

        private void Update()
        {
            timer += Time.deltaTime;
            float t = timer / duration;

            if (t >= 1f)
            {
                ReturnToPool();
                return;
            }

            // Float upward
            transform.position = startPos + Vector3.up * (floatSpeed * timer);

            // Scale down
            float scale = Mathf.Lerp(scaleStart, scaleEnd, t);
            transform.localScale = Vector3.one * scale;

            // Fade out
            if (textMesh != null)
            {
                Color c = startColor;
                c.a = 1f - t;
                textMesh.color = c;
            }

            // Billboard - face camera
            if (Camera.main != null)
            {
                transform.rotation = Camera.main.transform.rotation;
            }
        }

        private void ReturnToPool()
        {
            if (ownerPool != null)
                ownerPool.Return(gameObject);
            else
                gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            timer = 0f;
        }
    }
}
