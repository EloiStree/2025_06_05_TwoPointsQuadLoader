using UnityEngine;

namespace Eloi.TwoPoints {
    public class TwoPointsMono_SetStartStopPointsAtPosition : MonoBehaviour {
        public Transform m_whereToSetThePoint;
        public Transform m_pointStart;
        public Transform m_pointStop;
        private void Reset()
        {
            int childCount = transform.childCount;
            if (childCount > 0)
            {
                m_pointStart = transform.GetChild(0);
            }
            if (childCount > 1)
            {
                m_pointStop = transform.GetChild(1);
            }
            if (childCount > 2)
            {
                m_whereToSetThePoint = transform.GetChild(2);
            }
        }
        [ContextMenu("Set Start Point")]
        public void SetStartPoint()
        {
            if (m_whereToSetThePoint == null || m_pointStart == null)
            {
                return;
            }
            m_pointStart.position =m_whereToSetThePoint.position ;
        }
        [ContextMenu("Set Stop Point")]
        public void SetStopPoint()
        {
            if (m_whereToSetThePoint == null || m_pointStop == null)
            {
                return;
            }
            m_pointStop.position=m_whereToSetThePoint.position ;
        }

    }

}