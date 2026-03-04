using UnityEngine;

namespace PlanetDefense.Ship
{
    public class ShipOrbit : MonoBehaviour
    {
        [SerializeField] private Transform orbitCenter;
        [SerializeField] private float orbitRadius = 3f;
        [SerializeField] private float orbitSpeed = 30f;
        [SerializeField] private float startAngle;

        private float currentAngle;

        public float OrbitRadius
        {
            get => orbitRadius;
            set => orbitRadius = value;
        }

        public float OrbitSpeed
        {
            get => orbitSpeed;
            set => orbitSpeed = value;
        }

        public void Initialize(Transform center, float radius, float speed, float angle)
        {
            orbitCenter = center;
            orbitRadius = radius;
            orbitSpeed = speed;
            startAngle = angle;
        }

        private void Start()
        {
            currentAngle = startAngle;
            if (orbitCenter != null)
                UpdatePosition();
        }

        private void Update()
        {
            if (orbitCenter == null) return;

            currentAngle += orbitSpeed * Time.deltaTime;
            if (currentAngle >= 360f) currentAngle -= 360f;

            UpdatePosition();
        }

        private void UpdatePosition()
        {
            float rad = currentAngle * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(
                Mathf.Cos(rad) * orbitRadius,
                Mathf.Sin(rad) * orbitRadius,
                0f
            );
            transform.position = orbitCenter.position + offset;
        }
    }
}
