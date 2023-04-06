using EasyState.Core.Layers;
using EasyState.Core.Models;
using System;

namespace EasyState.Core.Utility
{
    public static class CoreExtensions
    {
        public static BaseLayer<T> Layer<T>(this Design design) where T : Model
        {
            Type type = typeof(T);

            if (type == typeof(Node))
            {
                return design.Nodes as BaseLayer<T>;
            }
            //if(type == typeof(Connection))
            //{
            //    return design.Connections as BaseLayer<T>;
            //}
            if (type == typeof(Note))
            {
                return design.Notes as BaseLayer<T>;
            }
            throw new NotImplementedException("There is not support for a layer of that type.");
        }
    }
}