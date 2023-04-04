using EasyState.Core.Models;
using EasyState.Editor.Renderers.Designs;
using EasyState.Editor.Templates;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using EasyState.Editor.Utility;
using EasyState.Core.Utility;

namespace EasyState.Editor.Renderers.Nodes
{
    public class NodeLayerRenderer : ModelRenderer<Design>, IDisposable
    {
        public override VisualElement Element { get; protected set; }
        private readonly Dictionary<Group, GroupRenderer> _groupRegistry = new Dictionary<Group, GroupRenderer>();
        private readonly Dictionary<Node, IDisposableElement> _nodeRegistry = new Dictionary<Node, IDisposableElement>();

        public NodeLayerRenderer(Design model, VisualElement container) : base(model)
        {
            Model.Nodes.Added += Nodes_Added;
            Model.Nodes.Removed += Nodes_Removed;
            Model.GroupAdded += Design_GroupAdded;
            Model.GroupRemoved += Design_GroupRemoved;
            Model.GroupUngrouped += Design_GroupUngrouped;
            Element = container;
            foreach (var node in Model.Nodes)
            {
                Nodes_Added(node);
            }
            foreach (var group in Model.Groups)
            {
                Design_GroupAdded(group);
            }
        }
        public override void Dispose()
        {
            Model.Nodes.Added -= Nodes_Added;
            Model.Nodes.Removed -= Nodes_Removed;
            Model.GroupRemoved -= Design_GroupRemoved;
            Model.GroupAdded -= Design_GroupAdded;
            foreach (var item in _nodeRegistry)
            {
                item.Value.Dispose();
            }
            foreach (var item in _groupRegistry)
            {
                item.Value.Dispose();
            }
        }
        private void Design_GroupAdded(Group group)
        {
            var groupElement = TemplateFactory.CreateGroupTemplate();
            var renderer = new GroupRenderer(group, groupElement, this);
            _groupRegistry.Add(group, renderer);
            Element.Insert(0, groupElement);
        }

        private void Design_GroupRemoved(Group group)
        {
            if (_groupRegistry.TryGetValue(group, out GroupRenderer renderer))
            {
                renderer.Dispose();
                renderer.Element.RemoveFromHierarchy();
                _groupRegistry.Remove(group);
            }
        }

        private void Design_GroupUngrouped(Group group)
        {
            Design_GroupRemoved(group);
        }

        private void Nodes_Added(Node node)
        {
            var template = TemplateFactory.CreateNode();
            var nodeElement = template.Q<VisualElement>("node");
            if (!node.IsJumpNode)
            {
                var nodeRenderer = new NodeRenderer(node, Model, nodeElement);
                _nodeRegistry[node] = nodeRenderer;
                Element.Add(nodeRenderer);

            }
            else
            {
                var splitIconWrapper = nodeElement.Q<VisualElement>("SplitIcon");
                var splitIcon = splitIconWrapper.Q<VisualElement>("split-icon");
                var openIcon = AssetDatabase.LoadAssetAtPath<Texture2D>( FilePaths.Combine(FilePaths.EditorImageFolder, "OpenIcon.png"));
                splitIcon.style.backgroundImage = new StyleBackground(openIcon);
                splitIcon.style.width = 17;
                splitIcon.style.height = 20;
                var splitParent = splitIcon.parent;
                splitParent.SetBorderRadius(0);
                splitParent.tooltip = "Jump to design";
                splitParent.SetBorderColor(Color.clear);
                var renderer = new JumpNodeRenderer(node, nodeElement, Model);
                _nodeRegistry[node] = renderer;
                Element.Add(renderer);
            }
        }

        private void Nodes_Removed(Node nodeToDelete)
        {
            var nodeRenderer = _nodeRegistry[nodeToDelete];
            nodeRenderer.Dispose();
            nodeRenderer.GetElement().RemoveFromHierarchy();
            _nodeRegistry.Remove(nodeToDelete);
        }
    }
}