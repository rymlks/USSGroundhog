using System.Collections.Generic;
using System.Linq;
using KinematicCharacterController;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace StaticUtils
{
    public class UnityUtil
    {
        protected static Color NullColor = new (0f, 0f, (1f/256f), 0f);

        public static Color GetNullColor()
        {
            return NullColor;
            
        }

        public static ColorBlock getBlockWithHighlightColorChanged(ColorBlock block, Color newColor)
        {
            block.highlightedColor = newColor;
            return block;
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

        public static void MoveAndRotatePlayer(Vector3 distance, Quaternion rotation, KinematicCharacterMotor motor, Camera camera)
        {
            motor.SetPositionAndRotation(motor.TransientPosition + distance, motor.TransientRotation * rotation);
            camera.transform.rotation = motor.TransientRotation * rotation;
            camera.GetComponent<FinalCharacterCamera>().SetFollowTransform(motor.GetComponent<FinalCharacterController>().CameraFollowPoint);
        }

        public static void MoveAndRotatePlayer(Vector3 distance, Vector3 rotation, KinematicCharacterMotor motor, Camera camera)
        {
            MoveAndRotatePlayer(distance, Quaternion.Euler(rotation), motor, camera);
        }
        
        public static bool TagAppearsInParent(GameObject baby, string tag)
        {
            List<GameObject> parents = GetParentGameObjects(baby);
            return parents.Any(parent => parent.CompareTag(tag));
        }
            
        public static GameObject FindParentWithTag(GameObject baby, string tag)
        {
            List<GameObject> parents = GetParentGameObjects(baby);
            return parents.FirstOrDefault(parent => parent.CompareTag(tag));
        }   

        public static int RandomNumberBetweenZeroAnd(int maxExclusive)
        {
            return UnityEngine.Random.Range(0, maxExclusive);
        }
        

        public static Transform SelectRandomChild(Transform parent)
        {
            return parent.GetChild(RandomNumberBetweenZeroAnd(parent.childCount));
        }

        public static Quaternion RandomQuaternion()
        {
            return UnityEngine.Random.rotation;
        }

        public static Vector3 getPlayerPosition()
        {
            return Object.FindObjectOfType<FinalCharacterController>().gameObject.transform.position;
        }
        
        public static Transform getCameraTransform()
        {
            return Object.FindObjectOfType<FinalCharacterCamera>().gameObject.transform;
        }
    }
}
