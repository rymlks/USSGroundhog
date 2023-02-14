using System.Collections.Generic;
using System;
namespace EasyState.Models.Resolvers
{
    /// <summary>
    /// Handles states with multiple possible connections, will return first connection that is true or returns the fallback state.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MultiConnectionResolver<T> : IResolver<T> where T : DataTypeBase
    {

        private readonly List<IConditionConnection<T>> _connections; 
        private readonly State<T> _fallbackState;
        private readonly int _connectionCount;
       
        public MultiConnectionResolver(List<IConditionConnection<T>> connections, State<T> fallbackState)
        {
            _connections = connections?? throw new ArgumentNullException(nameof(connections));
            _fallbackState = fallbackState ?? throw new ArgumentNullException(nameof(fallbackState)); 
            _connectionCount = _connections.Count;
            
        }

        public State<T> Resolve(T data) {

            for (int i = 0; i < _connectionCount; i++)
            {
                if (_connections[i].Condition.Evaluate(data))
                {
                    return _connections[i].DestState;
                }
            }
            return _fallbackState;
        }
        
        
    }
}
