using System.Collections.Generic;
using System.Linq;
using KinematicCharacterController;
using UnityEngine;

namespace StaticUtils
{
    public class UnityUtil
    {
        protected static Color NullColor = new (0f, 0f, (1f/256f), 0f);

        public static Color GetNullColor()
        {
            return NullColor;
            
        }

        public static List<GameObject> GetImmediateChildGameObjects(GameObject parent)
        {
            List<GameObject> toReturn = new List<GameObject>();
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                toReturn.Add(parent.transform.GetChild(i).gameObject);
            }

            return toReturn;
        }
    
        public static List<GameObject> GetParentGameObjects(GameObject baby)
        {
            List<GameObject> toReturn = new List<GameObject>();
            GameObject current = baby;
            while (current.transform.parent != null && current.transform.parent != current.transform)
            {
                toReturn.Add(current.transform.parent.gameObject);
                current = current.transform.parent.gameObject;
            }
            return toReturn;
        }

        public static void MoveAndRotatePlayer(Vector3 distance, Quaternion rotation, KinematicCharacterMotor motor)
        {
            motor.SetPositionAndRotation(motor.TransientPosition + distance, motor.TransientRotation * rotation);
        }

        public static void MoveAndRotatePlayer(Vector3 distance, Vector3 rotation, KinematicCharacterMotor motor)
        {
            motor.SetPositionAndRotation(motor.TransientPosition + distance, motor.TransientRotation * Quaternion.Euler(rotation));
        }
        
        public static bool TagAppearsInParent(GameObject baby, string tag)
        {
            List<GameObject> parents = GetParentGameObjects(baby);
            return parents.Any(parent => parent.CompareTag(tag));
        }

        public static int RandomNumberBetweenZeroAnd(int maxExclusive)
        {
            return UnityEngine.Random.Range(0, maxExclusive);
        }

        public static Transform SelectRandomChild(Transform parent)
        {
            return parent.GetChild(RandomNumberBetweenZeroAnd(parent.childCount));
        }
    }
}
