using UnityEngine;

namespace Eloi.TwoPoints {
    public class  TwoPointsMono_FollowExact: MonoBehaviour
    {

        private void Reset()
        {

            m_toMove = transform;
        }
        public Transform m_target;
        public Transform m_toMove;
        public bool m_useUpdate;
        public bool m_useLateUpdate=true;
        public bool m_useRotation = true;
        public bool m_useScale = false;

        public void SetTarget(Transform target)
        {
            m_target = target;
        }

        public void Update()
        {
            if (m_useUpdate)
            {
                FollowTarget();
            }
        }

        
        public void LateUpdate()
        {
            if (m_useLateUpdate)
            {
                FollowTarget();
            }

        }
        private void FollowTarget()
        {
            if (m_target == null || m_toMove == null)
            {
                return;
            }
            m_toMove.position = m_target.position;
            if (m_useRotation)
                m_toMove.rotation = m_target.rotation;
            if (m_useScale)
                m_toMove.localScale = m_target.localScale;
        }

    }

}