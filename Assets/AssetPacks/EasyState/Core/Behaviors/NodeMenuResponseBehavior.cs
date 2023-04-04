using EasyState.Core.Models;
using EasyState.Settings;
using System;
using System.Linq;
using UnityEngine;

namespace EasyState.Core.Behaviors
{
    public class NodeMenuResponseBehavior : Behavior
    {
        private EasyStateSettings _settings = EasyStateSettings.Instance;

        public NodeMenuResponseBehavior(Design design) : base(design)
        {
            Design.ContextMenuResponse += Design_ContextMenuResponse;
        }

        public override void Dispose()
        {
            Design.ContextMenuResponse -= Design_ContextMenuResponse;
        }

        private void BuildNode(string presetName, Vector2 pos)
        {
            var nodePos = Design.GetRelativePosition(pos) - Design.ToolbarOffset;
            var newNode = _settings.NodePresetCollection.BuildNodeFromPreset(presetName);
            if (newNode is null)
            {
                Debug.LogError("Trying to create a node from a preset that does not exist: " + presetName);
            }
            newNode.SetPositionSilently(nodePos);
            Design.Nodes.Add(newNode);
        }
        private void BuildJumpNode(Vector2 pos)
        {
            var nodePos = Design.GetRelativePosition(pos) - Design.ToolbarOffset;
            var newNode = new Node(nodePos, isJumpNode: true);
            newNode.SelectedNodeColor = _settings.NodePresetCollection.SelectedJumpNodeColor;
            newNode.NodeColor = _settings.NodePresetCollection.JumpNodeColor;

            newNode.Name = "Jumper";
            newNode.SetPositionSilently(nodePos);
            Design.Nodes.Add(newNode);
        }

        private void CreateTransition()
        {
            var selectNodes = Design.GetSelectedNodes();
            if (selectNodes.Count() == 0 || selectNodes.Count() > 1)
            {
                throw new InvalidOperationException("Expecting a single node to be selected when making a transition");
            }
            Design.OnCreateConnection(selectNodes.First());
        }

        private void DeleteNode()
        {
            Design.Nodes.Remove(Design.GetSelectedNodes());
        }

        private void Design_ContextMenuResponse(ContextMenuResponse response, Vector2 screenPoint, string data)
        {
            switch (response)
            {
                case ContextMenuResponse.CreateNode:
                    BuildNode(data, screenPoint);
                    break;



                case ContextMenuResponse.DeleteNode:
                    DeleteNode();
                    break;

                case ContextMenuResponse.CreateTransition:
                    CreateTransition();
                    break;
                case ContextMenuResponse.CreateJumper:
                    BuildJumpNode(screenPoint);
                    break;
            }
        }


    }
}