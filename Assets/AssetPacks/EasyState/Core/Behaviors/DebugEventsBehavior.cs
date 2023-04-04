using EasyState.Core.Models;
using UnityEngine;
using UnityEngine.UIElements;

namespace EasyState.Core.Behaviors
{
    internal class DebugEventsBehavior : Behavior
    {
        public DebugEventsBehavior(Design design) : base(design)
        {
            // Design.Changed += Design_Changed;
            Design.Nodes.Added += Nodes_Added;
            Design.Nodes.Removed += Nodes_Removed;
            Design.Notes.Added += Notes_Added;
            Design.Notes.Removed += Notes_Removed;
            Design.SelectionChanged += Design_SelectionChanged;
            Design.ZoomChanged += Design_ZoomChanged;
            Design.CreateConnection += Design_CreateConnection;
            Design.RightMouseUp += Design_RightMouseUp;
            Design.ContextMenuRequest += Design_ContextMenuRequest;
            Design.DetailsPanelRequested += Design_DetailsPanelRequested;
        }

        public override void Dispose()
        {
            //Design.Changed -=Design_Changed;
            Design.Nodes.Added -= Nodes_Added;
            Design.Nodes.Removed -= Nodes_Removed;
            Design.SelectionChanged -= Design_SelectionChanged;
            Design.ZoomChanged -= Design_ZoomChanged;
            Design.CreateConnection -= Design_CreateConnection;
            Design.RightMouseUp -= Design_RightMouseUp;
            Design.ContextMenuRequest -= Design_ContextMenuRequest;
            Design.Notes.Added -= Notes_Added;
            Design.Notes.Removed -= Notes_Removed;
            Design.DetailsPanelRequested -= Design_DetailsPanelRequested;
        }

        private void Design_Changed()
        {
            Design.OnDebugMessage(this, "Changed");
        }

        private void Design_ContextMenuRequest(ContextMenuType menuType, Vector2 screenPoint, SelectableModel model)
        {
            Design.OnDebugMessage(this, $"Context Menu Request: {menuType}, {screenPoint}");
        }

        private void Design_CreateConnection(Node node)
        {
            Design.OnDebugMessage(this, $"Create Connection - {node}");
        }

        private void Design_DetailsPanelRequested(Model obj)
        {
            Design.OnDebugMessage(this, "Details requested for " + obj.Name ?? obj.Id);
        }

        private void Design_RightMouseUp(Model arg1, MouseUpEvent arg2)
        {
            Design.OnDebugMessage(this, $"Right mouse up evt");
        }

        private void Design_SelectionChanged(SelectableModel obj)
        {
            Design.OnDebugMessage(this, $"SelectionChanged, Model={obj.GetType().Name}, Selected={obj.Selected}");
        }

        private void Design_ZoomChanged()
        {
            Design.OnDebugMessage(this, $"ZoomChanged, Zoom={Design.Zoom}");
        }

        private void Nodes_Added(Node obj)
        {
            Design.OnDebugMessage(this, $"Nodes.Added, Nodes=[{obj}]");
        }

        private void Nodes_Removed(Node obj)
        {
            Design.OnDebugMessage(this, $"Nodes.Removed, Nodes=[{obj}]");
        }

        private void Notes_Added(Note obj)
        {
            Design.OnDebugMessage(this, "Note created");
        }

        private void Notes_Removed(Note obj)
        {
            Design.OnDebugMessage(this, "Note removed");
        }
    }
}