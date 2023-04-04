using EasyState.Core.Models;
using EasyState.Settings;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace EasyState.Editor.Utility
{
    public class ShortcutManager : IDisposable
    {
        public Design CurrenDesign;
        private readonly VisualElement _content;

        public ShortcutManager(VisualElement root)
        {

            _content = root.Q<VisualElement>("content");
            _content.RegisterCallback<KeyDownEvent>(OnKeyDown, TrickleDown.TrickleDown);
        }

        public void Dispose()
        {
            _content.UnregisterCallback<KeyDownEvent>(OnKeyDown, TrickleDown.TrickleDown);
        }

        private void HandleGrouping(KeyDownEvent evt)
        {
            if (evt.actionKey && evt.keyCode == UnityEngine.KeyCode.G)
            {
                var selectedNodes = CurrenDesign.GetSelectedNodes();
                if (selectedNodes.Any())
                {
                    CurrenDesign.OnContextMenuResponse(ContextMenuResponse.CreateGroup, Vector2.zero);
                }
                evt.StopImmediatePropagation();
            }
        }
        private void HandleSaveDesign(KeyDownEvent evt)
        {
            if (evt.actionKey && evt.keyCode == KeyCode.S && !evt.shiftKey)
            {
                CurrenDesign.SaveChanges();
                evt.StopImmediatePropagation();
                
            }
        }
        private void HandleDuplicate(KeyDownEvent evt)
        {
            if (evt.actionKey && evt.keyCode == KeyCode.D)
            {
                CurrenDesign.OnContextMenuResponse(ContextMenuResponse.Duplicate, Vector2.zero);
                evt.StopImmediatePropagation();
            }
        }
        private void HandleValidate(KeyDownEvent evt)
        {
            if (evt.actionKey && evt.keyCode == KeyCode.V)
            {
                CurrenDesign.OnValidateDesign();
                evt.StopImmediatePropagation();
            }
        }
        private void HandleRebuildDesigner(KeyDownEvent evt)
        {
            if (evt.actionKey && evt.keyCode == KeyCode.R)
            {
                var window = EditorWindow.GetWindow<DesignerWindow>();
                window.OnCreateGUI();
                evt.StopImmediatePropagation();
            }
        }
        private void OnKeyDown(KeyDownEvent evt)
        {
            if (CurrenDesign == null)
            {
                return;
            }
            HandleGrouping(evt);
            HandleSaveDesign(evt);
            HandleDuplicate(evt);
            HandleValidate(evt);
            HandleRebuildDesigner(evt);
            if (evt.keyCode == KeyCode.Delete)
            {
                var selectedModels = CurrenDesign.GetSelectedModels().ToList();
                CurrenDesign.Batch(() =>
                {
                    foreach (var model in selectedModels)
                    {
                        if (model is Node)
                        {
                            var node = model as Node;
                            if (node.Name != NodePresetCollection.ENTRY_NODE)
                            {
                                CurrenDesign.OnContextMenuResponse(ContextMenuResponse.DeleteNode, Vector2.zero);
                            }
                            else
                            {
                                CurrenDesign.OnShowToast("Can't delete entry node", 1000);
                            }
                        }
                        if (model is Connection)
                        {
                            CurrenDesign.OnContextMenuResponse(ContextMenuResponse.DeleteTransition, Vector2.zero);
                        }
                        if (model is Note)
                        {
                            CurrenDesign.OnContextMenuResponse(ContextMenuResponse.DeleteNote, Vector2.zero);
                        }
                        if (model is Group)
                        {
                            CurrenDesign.OnContextMenuResponse(ContextMenuResponse.DeleteGroup, Vector2.zero);
                        }
                    };
                });
            }
        }
    }
}