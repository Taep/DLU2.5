using System.Collections.Generic;
using UnityEngine;

namespace PlanetDefense.Utils
{
    public class ObjectPool : MonoBehaviour
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private int initialSize = 10;
        [SerializeField] private bool autoExpand = true;
        [SerializeField] private Transform poolContainer;

        private readonly Queue<GameObject> available = new Queue<GameObject>();
        private readonly HashSet<GameObject> inUse = new HashSet<GameObject>();

        public int ActiveCount => inUse.Count;
        public int AvailableCount => available.Count;

        private void Awake()
        {
            if (poolContainer == null)
                poolContainer = transform;

            Prewarm(initialSize);
        }

        public void Initialize(GameObject prefab, int size)
        {
            this.prefab = prefab;
            Prewarm(size);
        }

        private void Prewarm(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var obj = CreateNewObject();
                obj.SetActive(false);
                available.Enqueue(obj);
            }
        }

        public GameObject Get()
        {
            GameObject obj;

            if (available.Count > 0)
            {
                obj = available.Dequeue();
            }
            else if (autoExpand)
            {
                obj = CreateNewObject();
            }
            else
            {
                Debug.LogWarning($"[ObjectPool] Pool exhausted for {prefab.name}, no auto-expand.");
                return null;
            }

            obj.SetActive(true);
            inUse.Add(obj);
            return obj;
        }

        public GameObject Get(Vector3 position, Quaternion rotation)
        {
            var obj = Get();
            if (obj != null)
            {
                obj.transform.SetPositionAndRotation(position, rotation);
            }
            return obj;
        }

        public void Return(GameObject obj)
        {
            if (obj == null) return;

            if (!inUse.Remove(obj))
            {
                Debug.LogWarning($"[ObjectPool] Trying to return object not from this pool: {obj.name}");
                return;
            }

            obj.SetActive(false);
            obj.transform.SetParent(poolContainer);
            available.Enqueue(obj);
        }

        public void ReturnAll()
        {
            var toReturn = new List<GameObject>(inUse);
            foreach (var obj in toReturn)
            {
                Return(obj);
            }
        }

        private GameObject CreateNewObject()
        {
            var obj = Instantiate(prefab, poolContainer);
            obj.name = $"{prefab.name}_{available.Count + inUse.Count}";
            return obj;
        }
    }
}
