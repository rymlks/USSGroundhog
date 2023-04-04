using EasyState.DataModels;
using System;

namespace EasyState.Core.Models
{
    public abstract class Model  
    {
        public event Action Changed;
        public bool HasName => !string.IsNullOrEmpty(Name);
        public string Id { get; }
        public string Name { get; set; }

        /// <summary>
        /// Model from loaded data
        /// </summary>
        /// <param name="data"></param>
        public Model(ModelData data)
        {
            Id = data.Id;
            Name = data.Name;
        }

        /// <summary>
        /// New Model
        /// </summary>
        /// <param name="id"></param>
        protected Model(string id)
        {
            if (id == null)
            {
                Id = Guid.NewGuid().ToString();
            }
            else
            {
                Id = id;
            }
        }

        public virtual void Refresh() => Changed?.Invoke();

 
    }
}