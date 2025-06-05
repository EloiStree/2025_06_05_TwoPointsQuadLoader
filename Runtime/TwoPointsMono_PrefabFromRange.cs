using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Eloi.TwoPoints {
        public class TwoPointsMono_PrefabFromRange : MonoBehaviour
    {

        public List<SegmentToPrefab> m_prefabsToTrigger = new List<SegmentToPrefab>();
        public UnityEvent<GameObject> m_onPrefabFound;


        public void GetPrefabInRange(float value, out List<SegmentToPrefab> found) {

            found = new List<SegmentToPrefab>();
            foreach (var prefabInRange in m_prefabsToTrigger)
            {
                if (value >= prefabInRange.m_startRange && value <= prefabInRange.m_stopRange)
                {
                    if (prefabInRange.m_prefab != null)
                    {
                        found.Add(prefabInRange);
                    }
                }
            }
        }


        public void TryToTriggerPrefabInRange(float value)
        {
            GetPrefabInRange(value,out List <SegmentToPrefab> found);
            foreach (var prefabInRange in found)
            {
                if (prefabInRange.m_prefab != null)
                {
                    m_onPrefabFound?.Invoke(prefabInRange.m_prefab);
                }
            }
        }


    }

}