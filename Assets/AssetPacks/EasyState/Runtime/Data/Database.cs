using EasyState.DataModels;
using EasyState.Settings;
using System.Collections.Generic;
using UnityEngine;

namespace EasyState.Data
{
    public static class Database
    {
        private static readonly Dictionary<string, object> _dataCache = new Dictionary<string, object>();
        private const string DATATYPE_CACHE_ID = "data-type-collection-cache";
        private const string BEHAVIOR_CACHE_ID = "behavior-collection-cache";
        public static DesignData GetDesignData(string designID)
        {
            if (_dataCache.TryGetValue(designID, out var data))
            {
                return data as DesignData;
            }
            else
            {
                var db = new DesignDatabase(designID);
                var design = db.Load();
                _dataCache.Add(designID, design);
                return design;
            }
        }

        public static DataTypeCollection GetDataTypes()
        {
            if (_dataCache.TryGetValue(DATATYPE_CACHE_ID, out var data))
            {
                return data as DataTypeCollection;
            }
            else
            {
                var db = new DataTypeDatabase();
                var collection = db.Load();
                _dataCache.Add(DATATYPE_CACHE_ID, collection);
                return collection;
            }
        }
        public static BehaviorCollectionData LoadBehaviors()
        {
            //In builds we can cache behaviors as no new ones will be built
#if !UNITY_EDITOR
           if (_dataCache.TryGetValue(BEHAVIOR_CACHE_ID, out var data))
            {
                return data as BehaviorCollectionData;
            }
            else
            {
                var db = new BehaviorCollectionDatabase();
                var collection = db.Load();
                _dataCache.Add(BEHAVIOR_CACHE_ID, collection);
                return collection;
            }  
#else
            var db = new BehaviorCollectionDatabase();
            var collection = db.Load();
            _dataCache.Add(BEHAVIOR_CACHE_ID, collection);
            return collection;

#endif
        }
        public static void ClearCache() => _dataCache.Clear();
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void Reset()
        {
            ClearCache();
        }
#if !UNITY_EDITOR
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        public static void LoadCache()
        {
            LoadBehaviors();
            if (EasyStateSettings.Instance.CacheAllDesignsOnBuild)
            {
                var allDesigns = new DesignCollectionFullDatabase().Load();
                int designs = allDesigns.Count;
                for (int i = 0; i < designs; i++)
                {
                    var design = allDesigns[i];
                    _dataCache.Add(design.Id, design);
                }
            }
            GetDataTypes();
        }
#endif
    }
}
