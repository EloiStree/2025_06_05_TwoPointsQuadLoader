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
            m_pointA = transform;
            m_pointB = transform;
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