using System.Collections.Generic;

namespace EasyState.Models
{
    /// <summary>
    /// Temp generic data store to pass data between application layers
    /// </summary>
    public class DataGrabBag
    {
        public object this[string index]
        {
            get
            {
                if (_bag.TryGetValue(index, out object obj))
                {
                    return obj;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (_bag.TryGetValue(index, out object obj))
                {
                    _bag[index] = value;
                }
                else
                {
                    _bag.Add(index, value);
                }
            }
        }

        private readonly Dictionary<string, object> _bag = new Dictionary<string, object>();

        public T Get<T>(string index)
        {
            object result = this[index];
            if (result == null)
            {
                return default;
            }
            T data = (T)result;
            return data;
        }
    }
}