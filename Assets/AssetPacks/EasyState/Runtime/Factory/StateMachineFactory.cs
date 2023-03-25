using EasyState.Core;
using EasyState.Data;
using EasyState.DataModels;
using EasyState.Models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EasyState.Factory
{
    public class StateMachineFactory
    {
        protected static Dictionary<string, object> _stateMachineCache = new Dictionary<string, object>();
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void Reset()
        {
            _stateMachineCache.Clear();
        }
    }
    public class StateMachineFactory<T> : StateMachineFactory where T : DataTypeBase
    {

        public static List<State<T>> LoadStates(DesignData data, StateMachine<T> stateMachine)
        {
            var loadedDesigns = new List<DesignData> { data };

            List<State<T>> states;            
            if (_stateMachineCache.TryGetValue(data.Id, out object cachedStates))
            {
                states = cachedStates as List<State<T>>;
            }
            else
            {
                var stateFactory = new StateFactory<T>(stateMachine);
                var resolverFactory = new ResolverFactory<T>(stateMachine.DataInstance);
                states = data.Nodes.Select(x => stateFactory.BuildStateWithoutResolver(x, data.Id)).ToList();
                List<NodeData> allNodes = new List<NodeData>(data.Nodes);
                var jumperStates = new List<State<T>>();
                foreach (var state in states)
                {
                    if(state.StateType == StateType.Jumper)
                    {
                        var design = loadedDesigns.FirstOrDefault(x => x.Id == state.JumperMachineID);
                        if(design == null)
                        {
                            design = Database.GetDesignData(state.JumperMachineID);
                            if(design.DataTypeID == null)
                            {
                                throw new System.Exception("Jumper node is trying to jump to a design that no longer exists. Check design to make sure all " +
                                    "connected jumper nodes have valid destination.");
                            }
                            loadedDesigns.Add(design);
                            jumperStates.AddRange(design.Nodes.Select(x=> stateFactory.BuildStateWithoutResolver(x, design.Id)));
                            allNodes.AddRange(design.Nodes);
                        }
                    }
                }
                states.AddRange(jumperStates);
                allNodes.ForEach(node =>
                {
                    var resolver = resolverFactory.BuildResolver(states, node, stateMachine);
                    var state = states.First(x => x.Id == node.Id);

                    state.Resolver = resolver;

                });


            }
            
            return states;
        }
      

    }
}
