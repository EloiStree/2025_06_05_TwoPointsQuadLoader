using UnityEngine;
using UnityEngine.Events;

namespace Eloi.TwoPoints {
    public class TwoPointsMono_CreatePrefabBetweenPoints : MonoBehaviour
    {
        public Transform m_startPoint;
        public Transform m_endPoint;
        public Transform m_parentToCreateIn;
        public GameObject m_prefabToCreate;
        public UnityEvent<GameObject> m_onPrefabCreated;


        public void SetPrefabAndCreate(GameObject toCreate)
        {
            SetPrefabToCreate(toCreate);
            CreatePrefabFromInspectorPrefab();
        }
        public void SetPrefabToCreate(GameObject toCreate)
        {
            m_prefabToCreate = toCreate;
        }

        [ContextMenu("Create Prefab From Inspector Prefab")]
        public void CreatePrefabFromInspectorPrefab()
        {
            if (m_startPoint == null || m_endPoint == null || m_prefabToCreate == null)
            {
                return;
            }
            Vector3 position = (m_startPoint.position + m_endPoint.position) / 2f;

            Vector3 up = Vector3.up;
            Vector3 directionX = (m_endPoint.position - m_startPoint.position);
            Vector3 forward = Vector3.Cross(directionX,up).normalized;

            Quaternion rotation = Quaternion.LookRotation(forward, Vector3.up);
            GameObject newPrefab = Instantiate(m_prefabToCreate, position, rotation);
            if (m_parentToCreateIn != null)
            {
                newPrefab.transform.SetParent(m_parentToCreateIn);
            }
            m_onPrefabCreated?.Invoke(newPrefab);

        }

    }

}