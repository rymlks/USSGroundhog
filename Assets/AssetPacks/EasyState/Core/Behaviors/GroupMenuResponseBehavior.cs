using EasyState.Core.Models;
using System;
using System.Linq;
using UnityEngine;

namespace EasyState.Core.Behaviors
{
    public class GroupMenuResponseBehavior : Behavior
    {
        public GroupMenuResponseBehavior(Design design) : base(design)
        {
            Design.ContextMenuResponse += Design_ContextMenuResponse;
        }

        public override void Dispose()
        {
            Design.ContextMenuResponse -= Design_ContextMenuResponse;
        }

        private void CreateGroup(Vector2 screenpoint)
        {
            var newGroup = new Group(Design.GetRelativePosition(screenpoint) - Design.ToolbarOffset);
            foreach (var model in Design.GetSelectedModels())
            {
                if (model is IGroupable)
                {
                    var groupable = model as IGroupable;

                    if (groupable.Group != null)
                    {
                        groupable.Group.RemoveChild(groupable);
                    }
                    newGroup.AddChild(groupable);
                }
            }
            Design.AddGroup(newGroup);
        }

        private void DeleteGroup()
        {
            var selectedGroup = Design.GetSelectedModels().FirstOrDefault();
            if (selectedGroup != null)
            {
                Design.RemoveGroup(selectedGroup as Group);
            }
        }

        private void Design_ContextMenuResponse(ContextMenuResponse response, UnityEngine.Vector2 screenpoint, string data)
        {
            switch (response)
            {
                case ContextMenuResponse.CreateGroup:
                    CreateGroup(screenpoint);
                    break;

                case ContextMenuResponse.DeleteGroup:
                    DeleteGroup();
                    break;

                case ContextMenuResponse.UngroupGroup:
                    UngroupGroup();
                    break;
                case ContextMenuResponse.UngroupNode:
                    UngroupNode();
                    break;
            }
        }
        private void UngroupNode()
        {
            foreach (var model in Design.GetSelectedModels())
            {
                if (model is IGroupable)
                {
                    var node = model as IGroupable;
                    if (node.Group != null)
                    {
                        node.Group.RemoveChild(node);
                    }
                }
            }
        }
        private void UngroupGroup()
        {
            var selectedGroup = Design.GetSelectedModels().First();
            if (selectedGroup is Group)
            {
                Design.UngroupGroup(selectedGroup as Group);
            }
            else
            {
                throw new InvalidOperationException("Trying to ungroup a group without a group being selected.");
            }
        }
    }
}