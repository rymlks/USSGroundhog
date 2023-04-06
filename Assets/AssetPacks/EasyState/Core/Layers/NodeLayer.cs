using EasyState.Core.Models;
using EasyState.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace EasyState.Core.Layers
{
    public class NodeLayer : BaseLayer<Node>
    {
        public NodeLayer(Design design) : base(design)
        {
        }

        public override bool Remove(Node node)
        {
            var result = base.Remove(node);
            if (result)
            {
                if (node.Group != null)
                {
                    node.Group.RemoveChild(node);
                }
                ClearConnectionDestValues(node);
            }
            return result;
        }

        public override void Remove(IEnumerable<Node> items)
        {
            items.GuardNullArg();
            Design.Batch(() =>
            {
                foreach (var item in items.ToList())
                {
                    if (item.IsEntryNode)
                    {
                        continue;
                    }
                    if (item.Group != null)
                    {
                        item.Group.RemoveChild(item);
                    }
                    if (_items.Remove(item))
                    {
                        OnItemRemoved(item);
                        TriggerRemovedEvent(item);
                        Design.OnModelDeleted(item);
                        ClearConnectionDestValues(item);
                    }
                }
            });
        }

        private void ClearConnectionDestValues(Node nodeBeingDeleted)
        {
            var connections = Design.Nodes.SelectMany(x => x.Connections).Where(x => x.DestNode == nodeBeingDeleted).ToList();

            foreach (var conn in connections)
            {
                conn.SourceNode.RemoveConnection(conn);
            }
        }
    }
}