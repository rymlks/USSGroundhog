using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using EasyState.Models;
namespace EasyState.Data
{
    public enum CachedInstanceType { Action, Condition, Evaluator, DataType }

    public class TypePoolCache
    {
        public static TypePoolCache DefaultCache
        {
            get
            {
                if (_defaultCache == null)
                {
                    _defaultCache = new TypePoolCache();
                }
                return _defaultCache;
            }
        }

        private static TypePoolCache _defaultCache;

        private Dictionary<CachedInstanceType, List<object>> _cachedDictionary = new Dictionary<CachedInstanceType, List<object>>();
        

        public List<T> GetAll<T>(CachedInstanceType type) where T : class
        {
            List<object> objects;
            bool exists = _cachedDictionary.TryGetValue(type, out objects);
            if (exists)
            {
                return objects.Select(x => x as T).ToList();
            }
            else
            {
                return null;
            }
        }

        public object GetObjectInstance(string typeName)
        {
            Type type = GetTypeFromString(typeName);
            CachedInstanceType key = GetCachedType(type);
            return GetObjectInstance(key, type);
        }

        public object GetObjectInstance(CachedInstanceType cachedType, Type type)
        {
            object obj = default;
            List<object> objects;
            bool alreadyCachedType = _cachedDictionary.TryGetValue(cachedType, out objects);
            if (alreadyCachedType)
            {
                obj = objects.FirstOrDefault(x => x.GetType() == type);
                if (obj == null)
                {
                    if (cachedType is CachedInstanceType.DataType)
                    {
                        obj = ScriptableObject.CreateInstance(type);
                        objects.Add(obj);
                    }
                    else
                    {
                        obj = Activator.CreateInstance(type);
                        objects.Add(obj);
                    }
                }
            }
            else
            {
                if (cachedType is CachedInstanceType.DataType)
                {
                    obj = ScriptableObject.CreateInstance(type);
                    _cachedDictionary.Add(cachedType, new List<object> { obj });
                }
                else
                {
                    obj = Activator.CreateInstance(type);
                    _cachedDictionary.Add(cachedType, new List<object> { obj });
                }
            }
            return obj;
        }

        private static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                {
                    return true;
                }
                toCheck = toCheck.BaseType;
            }
            return false;
        }

        private CachedInstanceType GetCachedType(Type type)
        {
            if (IsSubclassOfRawGeneric(typeof(Models.Action<>), type))
            {
                return CachedInstanceType.Action;
            }
            if (IsSubclassOfRawGeneric(typeof(Condition<>), type))
            {
                return CachedInstanceType.Condition;
            }
            if (IsSubclassOfRawGeneric(typeof(Evaluator<>), type))
            {
                return CachedInstanceType.Evaluator;
            }
            if (type?.BaseType == typeof(DataTypeBase))
            {
                return CachedInstanceType.DataType;
            }
            throw new Exception("Invalid type!");
        }

        public static Type GetTypeFromString(string dataTypeName)
        {
            Type dataType = Type.GetType(dataTypeName);
            if (dataType != null)
            {
                return dataType;
            }
            else
            {
                throw new InvalidOperationException("Could not find type : " + dataTypeName);
            }
//            DirectoryInfo parentFolder = Directory.GetParent("Assets");
//#if UNITY_EDITOR
//            string assembliesFolder = Path.Combine(parentFolder.FullName, "Library", "ScriptAssemblies");
//#else
//            string assembliesFolder = Path.Combine( Application.dataPath, "Managed");

//#endif
//            string[] assemblies = Directory.GetFiles(assembliesFolder, "*.dll");
//            foreach (var asm in assemblies)
//            {
//                if (asm.Contains("Unity."))
//                {
//                    continue;
//                }
//                dataType = Type.GetType(dataTypeName + "," + Path.GetFileNameWithoutExtension(asm));
//                if (dataType != null)
//                {
//                    return dataType;
//                }
//            }
        }
    }
}