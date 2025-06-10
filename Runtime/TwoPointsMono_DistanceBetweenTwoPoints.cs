using UnityEngine;

namespace Eloi.TwoPoints {
    [ExecuteInEditMode]
    public class TwoPointsMono_DistanceBetweenTwoPoints : MonoBehaviour {
        public Transform m_pointA;
        public Transform m_pointB;
        public float m_distance;
        public bool m_useDebugDraw = true;
        public Color m_debugDrawColor = Color.yellow;
        private void Reset()
        {
            
            int childCount = transform.childCount;
            if (childCount >0)
            {
                m_pointA = transform.GetChild(0);
            }
            if (childCount > 1)
            {
                m_pointB = transform.GetChild(1);
            }

        }
        public float GetDistance()
        {
            if (m_pointA == null || m_pointB == null)
            {
                return 0f;
            }
            return Vector3.Distance(m_pointA.position, m_pointB.position);
        }
        private void Update()
        {
            if (m_pointA == null || m_pointB == null)
            {
                return;
            }
            m_distance = GetDistance();
            if (m_useDebugDraw)
            {
                Debug.DrawLine(m_pointA.position, m_pointB.position, m_debugDrawColor);
            }
        }
    }

}