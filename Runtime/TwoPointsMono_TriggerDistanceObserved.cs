using System;
using UnityEngine;
using UnityEngine.Events;

namespace Eloi.TwoPoints {
    public class TwoPointsMono_TriggerDistanceObserved : MonoBehaviour
    {
        public float m_currentDistance;
        public UnityEvent<float> m_onDistanceTrigger;

        public void SetDistance(float distance)
        {
            m_currentDistance = distance;
        }

        public void SetAndTriggerDistance(float distance)
        {
            SetDistance(distance);
            TriggerDistance();
        }

        [ContextMenu("Trigger Distance")]
        public void TriggerDistance()
        {
            m_onDistanceTrigger?.Invoke(m_currentDistance);
        }


    }

}