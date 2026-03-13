using UnityEngine;

namespace PlanetDefense.Utils
{
    public class SkyboxRotator : MonoBehaviour
    {
        [SerializeField] private float rotationSpeed = 0.5f;
        [SerializeField] private Vector3 rotationAxis = new Vector3(0.1f, 1f, 0.05f);

        private void Update()
        {
            transform.Rotate(rotationAxis.normalized, rotationSpeed * Time.deltaTime, Space.Self);
        }
    }
}
