using EasyState.Core.Models;
using EasyState.Settings;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;

namespace EasyState.Editor.Renderers.Connections
{
    public class ConnectionLayerRenderer : ModelRenderer<Design>
    {
        public ConnectionSettings ConnectionSettings => Model.Settings.ConnectionSettings;
        public override VisualElement Element { get; protected set; }
        private readonly Dictionary<Connection, ConnectionRenderer> _connectionRegistry = new Dictionary<Connection, ConnectionRenderer>();

        public ConnectionLayerRenderer(Design model, VisualElement container) : base(model)
        {
            Element = container;
            model.Nodes.Added += Nodes_Added;
            model.Nodes.Removed += Nodes_Removed;
            foreach (var node in model.Nodes)
            {
                node.ConnectionAdded += Connections_Added;
                node.ConnectionDeleted += Connections_Removed;
            }
            var connections = model.Nodes.SelectMany(x => x.Connections);
            foreach (var conn in connections)
            {
                Connections_Added(conn);
            }
        }

        public override void Dispose()
        {
        }
        private void Connections_Added(Connection connection)
        {
            ConnectionRenderer connectionRenderer = new ConnectionRenderer(connection, new VisualElement(), this);
            _connectionRegistry[connection] = connectionRenderer;
            Element.Add(connectionRenderer);
        }

        private void Connections_Removed(Connection connection)
        {
            ConnectionRenderer connectionRenderer = _connectionRegistry[connection];
            connectionRenderer.Dispose();
            _connectionRegistry.Remove(connection);
            Element.Remove(connectionRenderer);
        }

        private void Nodes_Added(Node node)
        {
            node.ConnectionAdded += Connections_Added;
            node.ConnectionDeleted += Connections_Removed;
        }

        private void Nodes_Removed(Node node)
        {
            foreach (var conn in node.Connections)
            {
                Connections_Removed(conn);
            }
        }
    }
}