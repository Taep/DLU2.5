using UnityEngine;

namespace PlanetDefense.Ship
{
    /// <summary>
    /// 함선에 3D 모델 프리팹을 적용하는 컴포넌트.
    /// 기존 프리미티브 MeshRenderer/MeshFilter를 제거하고 모델 프리팹을 자식으로 생성합니다.
    /// </summary>
    public class ShipModelSetup : MonoBehaviour
    {
        [Header("모델 설정")]
        [Tooltip("적용할 3D 모델 프리팹 (GLB 임포트된 모델)")]
        [SerializeField] private GameObject shipModelPrefab;

        [Tooltip("모델 스케일 조정")]
        [SerializeField] private float modelScale = 0.3f;

        [Tooltip("모델 위치 오프셋 (로컬)")]
        [SerializeField] private Vector3 positionOffset = Vector3.zero;

        [Tooltip("모델 회전 오프셋 (오일러각)")]
        [SerializeField] private Vector3 rotationOffset = Vector3.zero;

        private GameObject modelInstance;

        private void Awake()
        {
            ApplyModel();
        }

        public void ApplyModel()
        {
            if (shipModelPrefab == null)
            {
                Debug.LogWarning($"[ShipModelSetup] {gameObject.name}: shipModelPrefab이 할당되지 않았습니다.");
                return;
            }

            // 기존 모델 인스턴스 제거
            if (modelInstance != null)
            {
                Destroy(modelInstance);
            }

            // 기존 프리미티브 렌더러 비활성화
            var meshRenderer = GetComponent<MeshRenderer>();
            if (meshRenderer != null)
                meshRenderer.enabled = false;

            // 모델 인스턴스 생성
            modelInstance = Instantiate(shipModelPrefab, transform);
            modelInstance.name = "ShipModel";
            modelInstance.transform.localPosition = positionOffset;
            modelInstance.transform.localRotation = Quaternion.Euler(rotationOffset);
            modelInstance.transform.localScale = Vector3.one * modelScale;
        }
    }
}
