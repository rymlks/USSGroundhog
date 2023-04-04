using EasyState.Core.Models;
using EasyState.Editor.Renderers.Designs;
using EasyState.Editor.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;

namespace EasyState.Editor.Debugger.Renderers
{
    internal class DebugDesignRenderer : IDisposable
    {
        private readonly Design _design;
        private readonly VisualElement _content;
        private readonly DesignRenderer _designRenderer;
        private List<SelectableModel> _modelsTraversedThisUpdate = new List<SelectableModel>();
        private Node _previousNode;
        
        public DebugDesignRenderer(Design design, VisualElement root)
        {
            _design = design;
            _design.AddDebugBehaviors();
            _design.UnselectAll();
            _content = root.Q<VisualElement>("content");
            VisualElement tabTemplate = TemplateFactory.CreateTabContent();
            _designRenderer = new DesignRenderer(design, tabTemplate, root, true);
            _content.Insert(0, _designRenderer);
            _content.RegisterCallback<GeometryChangedEvent>(RemoveNodeButtons);
            DebuggerWindow.Instance.EditorEnteredPlayMode += OnEditorEnteredPlayMode;
            DebuggerWindow.Instance.EditorExitedPlayMode += OnEditorExitPlayMode;
        }

        private void OnEditorExitPlayMode()
        {
            if (_design != null)
            {
                _design.UnselectAll();
            }
        }

        private void OnEditorEnteredPlayMode(EasyStateMachine stateMachineComponent)
        {
            if (stateMachineComponent != null && stateMachineComponent?.StateMachine != null)
            {

                stateMachineComponent.StateMachine.OnStateEntered += StateMachine_OnStateEntered;
                stateMachineComponent.StateMachine.OnUpdateCycleCompleted += StateMachine_OnUpdateCycleCompleted;
            }

        }

        private void StateMachine_OnUpdateCycleCompleted()
        {
            _design.UnselectAll();
            foreach (var model in _modelsTraversedThisUpdate)
            {
                _design.SelectModel(model, false);
            }
            _modelsTraversedThisUpdate.Clear();
        }

        private void StateMachine_OnStateEntered(Models.State stateEntered)
        {
            var node = _design.Nodes.First(x => x.Id == stateEntered.Id);
            _modelsTraversedThisUpdate.Add(node);
            if (_previousNode != null)
            {
                var connection = _previousNode.Connections.FirstOrDefault(x => x.DestNode == node);
                if (connection != null)
                {
                    _modelsTraversedThisUpdate.Add(connection);
                }
            }
            _previousNode = node;
        }

        private void RemoveNodeButtons(GeometryChangedEvent evt)
        {
            _content.UnregisterCallback<GeometryChangedEvent>(RemoveNodeButtons);
            _content.Query<TemplateContainer>("SplitIcon").ForEach(x => x.RemoveFromHierarchy());
        }

        public void Dispose()
        {
            _designRenderer.Dispose();
            DebuggerWindow.Instance.EditorEnteredPlayMode -= OnEditorEnteredPlayMode;
            DebuggerWindow.Instance.EditorExitedPlayMode -= OnEditorExitPlayMode;

        }
    }
}
