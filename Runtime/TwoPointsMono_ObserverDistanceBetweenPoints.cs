using System;
using UnityEngine;
using UnityEngine.Events;

namespace Eloi.TwoPoints {


    [System.Serializable]
    public class SegmentToPrefab {
        public GameObject m_prefab;
        public float m_startRange;
        public float m_stopRange;
    }

[ExecuteInEditMode]
public class TwoPointsMono_ObserverDistanceBetweenPoints : MonoBehaviour
{

    public Transform m_startPoint;
    public Transform m_endPoint;

    public float m_changedThreshold = 0.001f; 
    public float m_distanceBetweenPoints;
    public UnityEvent<float> m_onDistanceChanged;


        private void Awake()
        {
            if (m_startPoint == null || m_endPoint == null)
                return;
            
            m_distanceBetweenPoints = Vector3.Distance(m_startPoint.position, m_endPoint.position);
            m_onDistanceChanged?.Invoke(m_distanceBetweenPoints);
        }
        void Update()
        {

            CheckForDistance();

        }

    private void CheckForDistance()
    {
        if (m_startPoint == null || m_endPoint == null)
        {
              return;
        }
        float newDistance = Vector3.Distance(m_startPoint.position, m_endPoint.position);
        if (Mathf.Abs(newDistance - m_distanceBetweenPoints) > m_changedThreshold) 
        {
            m_distanceBetweenPoints = newDistance;
            m_onDistanceChanged?.Invoke(newDistance);
        }
    }
}

}