using UnityEngine;

namespace PlanetDefense.Ship
{
    public class ShipOrbit : MonoBehaviour
    {
        [SerializeField] private Transform orbitCenter;
        [SerializeField] private float orbitRadius = 3f;
        [SerializeField] private float orbitSpeed = 30f;
        [SerializeField] private float startAngle;

        [Header("3D Motion")]
        [SerializeField] private float bankAngle = 25f;          // 선회 기울기
        [SerializeField] private float bobAmplitude = 0.3f;      // 위아래 흔들림 크기
        [SerializeField] private float bobFrequency = 1.5f;      // 흔들림 속도
        [SerializeField] private float pitchOscillation = 10f;   // 앞뒤 기울기 진동
        [SerializeField] private float pitchFrequency = 0.8f;    // 기울기 진동 속도

        private float currentAngle;
        private float bobPhaseOffset;

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
            // 함선마다 다른 위상으로 흔들림이 겹치지 않게
            bobPhaseOffset = startAngle * 0.1f;
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
            float time = Time.time + bobPhaseOffset;

            // 기본 궤도 위치 (XY 평면)
            float x = Mathf.Cos(rad) * orbitRadius;
            float y = Mathf.Sin(rad) * orbitRadius;

            // Z축 상하 흔들림 (bobbing) - 입체감 부여
            float z = Mathf.Sin(time * bobFrequency) * bobAmplitude;

            transform.position = orbitCenter.position + new Vector3(x, y, z);

            // === 3D 회전 ===
            // 1) Yaw: 궤도 접선 방향 (진행 방향)
            float tangentAngle = currentAngle + 90f;

            // 2) Bank/Roll: 선회 시 안쪽으로 기울기 (비행기처럼)
            float roll = -bankAngle;

            // 3) Pitch: 앞뒤로 부드럽게 흔들림
            float pitch = Mathf.Sin(time * pitchFrequency) * pitchOscillation;

            // Z축 회전(진행방향) → X축 회전(pitch) → Y축 회전(bank)
            Quaternion baseRotation = Quaternion.Euler(0f, 0f, tangentAngle);
            Quaternion bankRotation = Quaternion.Euler(pitch, roll, 0f);
            transform.rotation = baseRotation * bankRotation;
        }
    }
}
