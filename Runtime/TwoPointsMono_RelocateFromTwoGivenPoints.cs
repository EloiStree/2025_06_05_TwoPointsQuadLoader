using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Eloi.TwoPoints {
    public class TwoPointsMono_RelocateFromTwoGivenPoints : MonoBehaviour
    {
        public Transform m_startPoint;
        public Transform m_endPoint;
        public float m_distanceThreshold = 0.1f;
        public List<TwoPointsMono_ToBeRelocated> m_relocatable= new List<TwoPointsMono_ToBeRelocated>();
        public List<GameObject> m_relocatableGameObjects = new List<GameObject>();


        [Header("Debug")]
        public float m_distanceBetweenPoints;
        public List<TwoPointsMono_ToBeRelocated> m_relocatableFound = new List<TwoPointsMono_ToBeRelocated>();

        [ContextMenu("Try to Relocate in Range")]
        public void TryToRelocateInRange()
        {
           
            float distanceBetweenPoints = Vector3.Distance(m_startPoint.position, m_endPoint.position);
            m_distanceBetweenPoints = distanceBetweenPoints;


            List<TwoPointsMono_ToBeRelocated> toCheck = new List<TwoPointsMono_ToBeRelocated>();
            toCheck.AddRange(m_relocatable);

            foreach(GameObject obj in m_relocatableGameObjects)
            {
                if (obj == null)
                    continue;
                TwoPointsMono_ToBeRelocated [] relocatables = obj.GetComponentsInChildren<TwoPointsMono_ToBeRelocated>();
                if (relocatables == null || relocatables.Length == 0)
                    continue;
                toCheck.AddRange(relocatables);
            }
            toCheck = toCheck.Distinct().ToList();
            m_relocatableFound=toCheck;

            foreach (var relocatable in toCheck)
            {
                if (relocatable == null || relocatable.m_whatToMove == null)
                    continue;
                float distanceBetweenRelocatePoints = Vector3.Distance(relocatable.m_startPoint.position, relocatable.m_endPoint.position);
                float deltaDistance = distanceBetweenPoints - distanceBetweenRelocatePoints;
                if (Mathf.Abs(deltaDistance) < m_distanceThreshold)
                    Relocate(m_startPoint, m_endPoint, relocatable);

            }
        }

        public bool m_flatOnXZ = true;
        public bool m_moveUpY = true;
        public void Relocate(Transform startPoint, Transform endPoint, TwoPointsMono_ToBeRelocated relocatable)
        {
            Vector3 startPointDestination = ( startPoint.position);
            Vector3 endPointDestination = (endPoint.position);
            Transform whatToRelocate = relocatable.m_whatToMove;
            Vector3 startPointOrigin = (relocatable.m_startPoint.position);
            Vector3 endPointOrigin = (relocatable.m_endPoint.position);
            Vector3 whatToRelocatePosition = (whatToRelocate.position);

            if (m_flatOnXZ)
            {
                FlatOnXZ(ref startPointDestination);
                FlatOnXZ(ref endPointDestination);
                FlatOnXZ(ref startPointOrigin);
                FlatOnXZ(ref endPointOrigin);
                FlatOnXZ(ref whatToRelocatePosition);
            }

            Vector3 middleDestination = (startPointDestination + endPointDestination) / 2f;
            Vector3 middleOrigin = (startPointOrigin + endPointOrigin) / 2f;

            Vector3 middleOrigneToDestination = middleDestination - middleOrigin;

            whatToRelocate.position += middleOrigneToDestination;

            Vector3 originDirection = (endPointOrigin - startPointOrigin).normalized;
            Vector3 destinationDirection = (endPointDestination - startPointDestination).normalized;
            
            Quaternion from = Quaternion.FromToRotation(Vector3.forward, originDirection);
            Quaternion to = Quaternion.FromToRotation(Vector3.forward, destinationDirection);

            RotateTargetAroundPointMath(
                whatToRelocate.position, whatToRelocate.rotation,
                middleDestination, from,to,
                out Vector3 newPosition, out Quaternion newRotation);

            whatToRelocate.position = newPosition;
            whatToRelocate.rotation = newRotation;

            if (m_moveUpY)
            {
                Vector3 midPoint = (relocatable.m_startPoint.position + relocatable.m_endPoint.position) / 2f;
                Vector3 midDestination = (startPoint.position + endPoint.position) / 2f;
                float heightDirection = midDestination.y - midPoint.y;
                whatToRelocate.position += new Vector3(0f, heightDirection, 0f);
            }
           






            //Vector3 dirMidToAnchor = startPointDestination-middleOrigin;
            //whatToRelocate.position += dirMidToAnchor;


            //Vector3 localDirectionDestination = (endPointDestination - startPointDestination).normalized;
            //Vector3 localDirectionOrigin = (endPointOrigin - startPointOrigin).normalized;
            //Quaternion rotationDestination = Quaternion.FromToRotation(localDirectionOrigin, localDirectionDestination);

            //Vector3 toMovePosition = relocatable.m_whatToMove.position - directionToStart;
            //Quaternion toRotateRotation = relocatable.m_whatToMove.rotation;
            //RotateAroundPivot(
            //    toMovePosition, toRotateRotation,
            //    startPointDestination, rotationDestination,
            //    out Vector3 newPosition, out Quaternion newRotation);
            //relocatable.m_whatToMove.position = newPosition;
            //relocatable.m_whatToMove.rotation = newRotation;


        }
        public Vector3 FlatOnXZ(Vector3 value)
        {
            value.y = 0f;
            return value;
        }
        public void FlatOnXZ(ref Vector3 value)
        {
            value.y = 0f;
         }

        public static void RotateAroundPivot(
         Vector3 whatToRotatePosition,
         Quaternion whatToRotateRotation,
         Vector3 centerRotation,
         Quaternion rotationToApply,
         out Vector3 newPosition,
         out Quaternion newRotation)

        {
            //Rotate the right point to in aim to reconstruct the forward direction
            Vector3 rightPoint = whatToRotatePosition + whatToRotateRotation * Vector3.right;
            Vector3 currentPointRelocate = RotatePointAroundPivot(whatToRotatePosition, centerRotation, rotationToApply);
            Vector3 rightPointRelocated = RotatePointAroundPivot(rightPoint, centerRotation, rotationToApply);
            Vector3 centerToPointDirection = currentPointRelocate - centerRotation;
            Vector3 pointToRightDirection = rightPointRelocated - currentPointRelocate;
            Vector3 newForwardDirection = Vector3.Cross(pointToRightDirection, centerToPointDirection).normalized;
            newPosition = currentPointRelocate;
            newRotation = Quaternion.LookRotation(newForwardDirection, centerToPointDirection);

        }
        public static void RotateTargetAroundPointMath(
          Vector3 postion, Quaternion rotation,
          Vector3 center, Quaternion rotationFrom, Quaternion rotationTo,
          out Vector3 newPosition, out Quaternion newRotation)
        {
            // Calculate the rotation difference (quaternion multiplication)
            Quaternion rotationDifference = rotationTo * Quaternion.Inverse(rotationFrom);

            // Calculate the direction from the center to the object to move
            Vector3 direction = postion - center;

            // Rotate the direction by the rotation difference
            Vector3 rotatedDirection = rotationDifference * direction;

            // Update the position of the object
            newPosition = center + rotatedDirection;

            // Apply the rotation difference to the object
            newRotation = rotationDifference * rotation;

        }
        public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion rotation)
        {
            return rotation * (point - pivot) + pivot;
        }

        public static void RotateTargetAroundPointByCreatingEmpyPoint(Transform whatToMove, Vector3 centroide, Quaternion rotationFrom, Quaternion rotationTo)
        {
            // THIS IS A CHEAT CODE THAT SHOULD NOT BE USED.
            // I WILL LOOK LATER AT THE NORMAL MATH WAY TO DO IT
            Quaternion toRotate = rotationTo * Quaternion.Inverse(rotationFrom);

            Transform p = whatToMove.parent;
            GameObject g = new GameObject("t");
            Transform t = g.transform;
            t.position = centroide;

            whatToMove.parent = t;
            t.rotation *= toRotate;
            whatToMove.parent = p;
            if (Application.isPlaying)
                GameObject.DestroyImmediate(g);
            else
                GameObject.Destroy(g);
        }

    }

}