using System;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;
using Managers;

public class MetaManager : MonoBehaviourSingleton, IMetaManager
{
    [SerializeField]protected List<MonoBehaviour> managers;

    public UnityEngine.MonoBehaviour GetManager<MonoBehaviour>(Predicate<UnityEngine.MonoBehaviour> manager)
    {
        return managers.Find(manager);
    }
}
