using EasyState.Core.Models;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace EasyState.Core.Behaviors
{
    internal class ConnectionMenuResponseBehavior : Behavior
    {
        private Node _sourceNode;

        public ConnectionMenuResponseBehavior(Design design) : base(design)
        {
            Design.MouseDown += Design_MouseDown;
            Design.CreateConnection += Design_CreateConnection;
            Design.ContextMenuResponse += Design_ContextMenuResponse;
        }

        public override void Dispose()
        {
            Design.MouseDown -= Design_MouseDown;
            Design.CreateConnection -= Design_CreateConnection;
            Design.ContextMenuResponse -= Design_ContextMenuResponse;
        }

        private void Design_ContextMenuResponse(ContextMenuResponse responseType, Vector2 p, string data)
        {
            if (responseType == ContextMenuResponse.DeleteTransition)
            {
                var selectedConnection = Design.GetSelectedModels().FirstOrDefault();
                if (selectedConnection == null)
                {
                    throw new InvalidOperationException("Expected connection to be selected but nothing is selected");
                }
                if (selectedConnection is Connection)
                {
                    var conn = selectedConnection as Connection;
                    conn.SourceNode.RemoveConnection(conn);
                }
                else
                {
                    throw new InvalidOperationException("Expected selected object to be a connection.");
                }
            }
        }

        private void Design_CreateConnection(Node node)
        {
            _sourceNode = node;
            Design.SelectModel(node, true);
            Design.State = DesignState.CreatingTransition;
        }

        private void Design_MouseDown(Model model, MouseDownEvent evt)
        {
            if (Design.State != DesignState.CreatingTransition)
            {
                return;
            }
            TryMakeConnection(model);
        }

        private void EndConnection()
        {
            Design.State = DesignState.Idle;
            _sourceNode = null;
        }

        private void TryMakeConnection(Model model)
        {
            if (!(model is Node))
            {
                EndConnection();
                return;
            }
            Node destNode = model as Node;
            if (!_sourceNode.CanAttachTo(destNode))
            {
                EndConnection();
                return;
            }
            var connection = new Connection(_sourceNode, destNode);
            _sourceNode.AddConnection(connection);
            EndConnection();
        }
    }
}