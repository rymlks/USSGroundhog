namespace EasyState.Models.Resolvers
{
    public class LeafResolver<T> : IResolver<T> where T : DataTypeBase
    {
        private readonly State<T> _entryState;

        public LeafResolver(State<T> entryState)
        {
            _entryState = entryState;
        }

        public State<T> Resolve(T data) => _entryState;
    }
}
