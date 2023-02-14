using EasyState.Core.Models;
using EasyState.Settings;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace EasyState.Editor.Renderers.Designs
{
    public class ContextMenuRenderer : IDisposable
    {
        private const float LABEL_WIDTH = 150;
        private readonly Design _design;
        private readonly VisualElement _menuBox;
        private readonly EasyStateSettings _settings = EasyStateSettings.Instance;
        private readonly VisualElement MenuContainer;
        private Clickable _clickRef;
        private Vector2 _menuScreenPoint;

        public ContextMenuRenderer(VisualElement menuContainer, Design design)
        {
            MenuContainer = menuContainer;
            _clickRef = new Clickable(HideMenu);
            MenuContainer.AddManipulator(_clickRef);
            MenuContainer.RegisterCallback<MouseDownEvent>(SwallowContextEvent);
            MenuContainer.RegisterCallback<MouseUpEvent>(SwallowContextEvent);
            _menuBox = new VisualElement();
            _menuBox.style.backgroundColor = new StyleColor(new Color(0.05490196f, 0.07450981f, 0.1137255f, 1));
            _menuBox.style.width = LABEL_WIDTH;

            MenuContainer.Add(_menuBox);
            _design = design;
            _design.ContextMenuRequest += ShowContextMenuRequest;
        }

        public void Dispose()
        {
            _design.ContextMenuRequest -= ShowContextMenuRequest;
            MenuContainer.UnregisterCallback<MouseDownEvent>(SwallowContextEvent);
            MenuContainer.UnregisterCallback<MouseUpEvent>(SwallowContextEvent);
            MenuContainer.RemoveManipulator(_clickRef);
            MenuContainer.Clear();
        }

        private void BuildBackgroundMenu()
        {
            var groupableNodes = _design.GetSelectedModels().Where(x=> x is IGroupable);
            if (groupableNodes.Any())
            {
                var groupable = groupableNodes.First() as IGroupable;
                bool firstNodeGrouped = groupable.Group != null;
                if (!firstNodeGrouped || groupableNodes.Count() > 1)
                {
                    _menuBox.Add(BuildMenuOption("Group Selected", ContextMenuResponse.CreateGroup.ToString()));
                }
            }
            if (_settings.NodePresetCollection.NodePresets is null)
            {
                Debug.LogWarning("There are no node presets created, add preset to settings object to add it to the design.");
            }
            else
            {
                foreach (var preset in _settings.NodePresetCollection.NodePresets)
                {
                    _menuBox.Add(BuildMenuOption("Create " + preset.PresetName, ContextMenuResponse.CreateNode.ToString(), preset.PresetName));
                }
            }

            _menuBox.Add(BuildMenuOption("Create Note", ContextMenuResponse.CreateNote.ToString()));
            _menuBox.Add(BuildMenuOption("Create Jumper", ContextMenuResponse.CreateJumper.ToString()));
        }

        private void BuildGroupMenu()
        {
            _menuBox.Add(BuildMenuOption("Ungroup Nodes", ContextMenuResponse.UngroupGroup.ToString()));
            _menuBox.Add(BuildMenuOption("Delete Group", ContextMenuResponse.DeleteGroup.ToString()));
            _menuBox.Add(BuildMenuOption("Duplicate Group", ContextMenuResponse.Duplicate.ToString()));
        }

        private Label BuildMenuOption(string text, string responseType, string data = null)
        {
            Label label = new Label(text);
            label.style.width = LABEL_WIDTH;
            label.AddToClassList("menu-item");
            label.AddManipulator(new Clickable(() => OnMenuItemClicked(responseType, data)));
            return label;
        }

        private void BuildNodeMenu()
        {
            var selectedNodes = _design.GetSelectedNodes().ToList();
            if (selectedNodes.Count > 1)
            {
                _menuBox.Add(BuildMenuOption("Group Nodes", ContextMenuResponse.CreateGroup.ToString()));
                _menuBox.Add(BuildMenuOption("Delete Nodes", ContextMenuResponse.DeleteNode.ToString()));
                var node = selectedNodes[0];
                if (!node.IsEntryNode)
                {
                    _menuBox.Add(BuildMenuOption("Duplicate Nodes", ContextMenuResponse.Duplicate.ToString()));
                }
            }
            else
            {
                var node = selectedNodes[0];
                if (node.Group != null)
                {
                    _menuBox.Add(BuildMenuOption("Ungroup Node", ContextMenuResponse.UngroupNode.ToString()));
                }
                if (!node.IsJumpNode)
                {
                    _menuBox.Add(BuildMenuOption("Create Transition", ContextMenuResponse.CreateTransition.ToString()));
                }
                if (!node.IsEntryNode)
                {
                    _menuBox.Add(BuildMenuOption("Delete Node", ContextMenuResponse.DeleteNode.ToString()));
                    _menuBox.Add(BuildMenuOption("Duplicate Node", ContextMenuResponse.Duplicate.ToString()));

                }
            }
        }

        private void BuildNoteMenu()
        {
            var selectedNote = _design.GetSelectedModels().FirstOrDefault(x => x is Note) as Note;
            if(selectedNote != null)
            {
                if(selectedNote.Group != null)
                {
                    _menuBox.Add(BuildMenuOption("Ungroup Note", ContextMenuResponse.UngroupNode.ToString()));
                }
            }
            _menuBox.Add(BuildMenuOption("Delete Note", ContextMenuResponse.DeleteNote.ToString()));
            _menuBox.Add(BuildMenuOption("Duplicate Note", ContextMenuResponse.Duplicate.ToString()));
        }

        private void HideMenu()
        {
            MenuContainer.style.display = DisplayStyle.None;
            _design.State = DesignState.Idle;
        }

        private void OnMenuItemClicked(string responseType, string data)
        {
            HideMenu();
            ContextMenuResponse response = (ContextMenuResponse)Enum.Parse(typeof(ContextMenuResponse), responseType);
            _design.OnContextMenuResponse(response, _menuScreenPoint, data);
        }

        private void ShowContextMenuRequest(ContextMenuType menuType, Vector2 screenPoint, SelectableModel model)
        {
            _design.State = DesignState.ContextMenuOpen;
            MenuContainer.style.display = DisplayStyle.Flex;
            _menuBox.Clear();
            _menuBox.transform.position = MenuContainer.WorldToLocal(screenPoint);
            _menuScreenPoint = screenPoint;
            switch (menuType)
            {
                case ContextMenuType.Background:
                    BuildBackgroundMenu();
                    break;

                case ContextMenuType.Node:
                    BuildNodeMenu();
                    break;

                case ContextMenuType.Group:
                    BuildGroupMenu();
                    break;

                case ContextMenuType.Note:
                    BuildNoteMenu();
                    break;
                case ContextMenuType.JumpNode:
                    BuildNodeMenu();
                    break;
                case ContextMenuType.Connection:
                    BuildConnectionMenu();
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        private void BuildConnectionMenu()
        {
            _menuBox.Add(BuildMenuOption("Delete Connection", ContextMenuResponse.DeleteTransition.ToString()));
        }

        private void SwallowContextEvent<T>(MouseEventBase<T> evt) where T : MouseEventBase<T>, new()
        {
            if (evt.button == 1)
            {
                HideMenu();
                evt.StopImmediatePropagation();
            }
        }
    }
}