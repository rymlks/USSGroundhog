using EasyState.DataModels;

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EasyState.Core.Models
{
    public class Group : MoveableModel, IDuplicable, IDataModel<GroupData>
    {
        public event Action<IGroupable> ModelAdded;

        public event Action<IGroupable> ModelRemoved;

        public IReadOnlyCollection<IGroupable> Children => new List<IGroupable>(_children);
        public Rect Rect { get; private set; }

        public bool CanDuplicate => true;

        public const string DEFAULT_GROUP_NAME = "New Group";
        private const float _HEADER_HEIGHT = 25f;
        private const float _PADDING = 20f;
        private readonly List<IGroupable> _children = new List<IGroupable>();

        public Group(Vector2? position = null) : base(position)
        {
        }

        public Group(GroupData data, IEnumerable<IGroupable> children) : base(data)
        {
            _children = new List<IGroupable>(children);
            foreach (IGroupable child in _children)
            {
                child.Group = this;
                child.Moving += OnChildNodeMoved;
            }
            CalculateRect();
        }

        public void AddChild(IGroupable node)
        {
            if (!_children.Contains(node))
            {
                _children.Add(node);
                ModelAdded?.Invoke(node);
                node.Moving += OnChildNodeMoved;
                node.Group = this;
                Refresh();
            }
        }

        public override void Refresh()
        {
            foreach (IGroupable item in Children)
            {
                item.OnGroupRefresh();
            }
            CalculateRect();
            base.Refresh();
        }

        public void RemoveChild(IGroupable node)
        {
            if (_children.Contains(node))
            {
                _children.Remove(node);
                node.Moving -= OnChildNodeMoved;
                ModelRemoved?.Invoke(node);
                node.Group = null;
                Refresh();
            }
        }

        public override void SetPosition(Vector2 position)
        {
            Vector2 delta = position - Position;
            base.SetPosition(position);
            foreach (IGroupable item in Children)
            {
                item.UpdatePosition(delta);
            }

            Refresh();
        }

        public override void UpdatePosition(Vector2 delta)
        {
            base.UpdatePosition(delta);
            foreach (Node item in Children)
            {
                item.UpdatePosition(delta);
            }
            Refresh();
        }

        private void CalculateRect()
        {
            if (!Children.Any())
            {
                return;
            }
            float minX = float.MaxValue;
            float maxX = float.MinValue;
            float minY = float.MaxValue;
            float maxY = float.MinValue;

            foreach (IGroupable node in _children)
            {
                if (node.Rect.xMin < minX)
                {
                    minX = node.Rect.xMin;
                }
                if (node.Rect.xMax > maxX)
                {
                    maxX = node.Rect.xMax;
                }
                if (node.Rect.yMin < minY)
                {
                    minY = node.Rect.yMin;
                }
                if (node.Rect.yMax > maxY)
                {
                    maxY = node.Rect.yMax;
                }
            }

            Rect = new Rect(minX - _PADDING, minY - _HEADER_HEIGHT - _PADDING, maxX - minX + (2 * _PADDING), maxY - minY + (2 * _PADDING) + _HEADER_HEIGHT);
            Position = Rect.position;
        }

        private void OnChildNodeMoved(MoveableModel obj)
        {
            CalculateRect();
            base.Refresh();
        }

        public IDuplicable Duplicate(Design design)
        {
            var groupData = Serialize();
            groupData.Id = Guid.NewGuid().ToString();

            var duplicatedGroup = new Group();
            design.AddGroup(duplicatedGroup);
            foreach (var item in Children)
            {
                if (item is IDuplicable iduplicable)
                {
                    if (iduplicable.CanDuplicate)
                    {
                        var duplicatedItem = iduplicable.Duplicate(design);

                        var groupableItem = (IGroupable)duplicatedItem;
                        duplicatedGroup.AddChild(groupableItem);
                    }

                }
            }
            if (duplicatedGroup.Children.Count == 0)
            {
                design.OnShowToast("Duplicated group was empty, deleting from design.", 4500);
                design.RemoveGroup(duplicatedGroup);
            }
            return duplicatedGroup;
        }

        public GroupData Serialize()
        {
            var data = new GroupData() {
                ChildrenIDs = Children.Select(x => x.Id).ToList()
            };
            data.SetModelData(this);
            return data;
        }
    }
}