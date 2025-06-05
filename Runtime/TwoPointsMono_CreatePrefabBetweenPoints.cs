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


        public float m_thresholdToBeHoriwzontal = 0.05f;
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

            float yStart = m_startPoint.position.y;
            float yEnd = m_endPoint.position.y;
            float heightBetween = Mathf.Abs(yStart - yEnd);
            bool isFlatHorizontal = heightBetween < m_thresholdToBeHoriwzontal;
            if (isFlatHorizontal)
            {
                Vector3 position = (m_startPoint.position + m_endPoint.position) / 2f;

                Vector3 up = Vector3.up;
                Vector3 directionX = (m_endPoint.position - m_startPoint.position);
                Vector3 forward = Vector3.Cross(directionX, up).normalized;

                Quaternion rotation = Quaternion.LookRotation(forward, Vector3.up);
                GameObject newPrefab = Instantiate(m_prefabToCreate, position, rotation);
                if (m_parentToCreateIn != null)
                {
                    newPrefab.transform.SetParent(m_parentToCreateIn);
                }
                m_onPrefabCreated?.Invoke(newPrefab);
            }
            else
            {
                Vector3 position = (m_startPoint.position + m_endPoint.position) / 2f;

                Vector3 up = Vector3.up;
                Vector3 directionX = (m_endPoint.position - m_startPoint.position);
                Vector3 horizontalAxis = Vector3.Cross(directionX, up).normalized;
                Vector3 forward = Vector3.Cross(horizontalAxis, directionX).normalized;

                Quaternion rotation = Quaternion.LookRotation(forward, Vector3.up);
                rotation = rotation * Quaternion.Euler(0, 0, 90); 
                GameObject newPrefab = Instantiate(m_prefabToCreate, position, rotation);
                if (m_parentToCreateIn != null)
                {
                    newPrefab.transform.SetParent(m_parentToCreateIn);
                }
                m_onPrefabCreated?.Invoke(newPrefab);
            }

            

        }

    }

}