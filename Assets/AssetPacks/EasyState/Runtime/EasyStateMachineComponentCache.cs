using System.Collections.Generic;

namespace EasyState.Runtime
{
    public static class EasyStateMachineComponentCache
    {
        private static Dictionary<string, EasyStateMachine> _stateMachineCache = new Dictionary<string, EasyStateMachine>();

        public static void AddComponent(EasyStateMachine component)
        {
            _stateMachineCache[component.StateMachineComponentID] = component;
        }
        public static EasyStateMachine GetComponent(string componentId)
        {
            if(_stateMachineCache.TryGetValue(componentId, out EasyStateMachine component))
            {
                return component;
            }
            return null;
        }

    }
}
