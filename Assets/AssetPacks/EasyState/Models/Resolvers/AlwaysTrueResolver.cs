using System;

namespace EasyState.Models.Resolvers
{
    public class AlwaysTrueResolver<T> : IResolver<T> where T : DataTypeBase
    {
        private readonly State<T> _destState;

        public AlwaysTrueResolver(State<T> destState)
        {
            _destState = destState ?? throw new ArgumentNullException(nameof(destState));
        }

        public State<T> Resolve(T data) => _destState;
    }
}
