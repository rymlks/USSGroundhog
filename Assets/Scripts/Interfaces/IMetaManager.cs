using System;

namespace Interfaces
{
    public interface IMetaManager
    {
        UnityEngine.MonoBehaviour GetManager<MonoBehaviour>(Predicate<UnityEngine.MonoBehaviour> manager);
    }
}