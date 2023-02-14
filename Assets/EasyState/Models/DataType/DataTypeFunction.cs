using System;

namespace EasyState.Models.DataType
{
    public abstract class DataTypeFunction<T> : IDataTypeFunction<T> where T : DataTypeBase
    {
        public abstract string Id { get; }
        public string Name { get; }

        protected DataTypeFunction(string name)
        {          
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }
        public IDataTypeFunction<T> AsFunction() => this; 
    }
}
