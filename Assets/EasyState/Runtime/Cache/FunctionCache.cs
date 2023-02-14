using EasyState.Data;
using EasyState.DataModels;
using EasyState.Models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EasyState.Cache
{
    public class FunctionCache
    {
        private static FunctionCache _instance;

        public static FunctionCache DefaultCache
        {
            get
            {
                if (_instance == null)
                {

                    _instance = new FunctionCache();
                }
                return _instance;
            }
        }
        private Dictionary<System.Type, object> _cache = new Dictionary<System.Type, object>();
        private readonly DataTypeCollection _dataTypeCollection;
        private FunctionCache()
        {
            _dataTypeCollection = Database.GetDataTypes();
        }

        public FunctionCacheSet<T> GetCache<T>(T dataTypeInstance) where T : DataTypeBase
        {
            var dataType = typeof(T);
            if (_cache.TryGetValue(dataType, out object cachedObject))
            {
                return cachedObject as FunctionCacheSet<T>;
            }
            else
            {
                var dataTypeData = _dataTypeCollection.DataTypes.First(x => x.AssemblyQualifiedName == dataType.AssemblyQualifiedName);
                var newCache = new FunctionCacheSet<T>(dataTypeInstance, dataTypeData);
                _cache.Add(dataType, newCache);

                return newCache;
            }
        }
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void Reset()
        {
            _instance?._cache.Clear();
            _instance = null;
        }
    }

}