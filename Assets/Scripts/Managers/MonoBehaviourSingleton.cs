using System;
using UnityEngine;

namespace Managers
{
    public class MonoBehaviourSingleton : MonoBehaviour
    {
        public Type instance;
        public void Initialize(Type type)
        {
            instance = type;
        }
    }
}