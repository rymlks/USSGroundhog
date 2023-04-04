using EasyState.Core;
using EasyState.Core.Models;
using EasyState.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace EasyState.Editor.Debugger.Renderers
{
    public class DebugDataRenderer : IDisposable
    {
        private class DataElement
        {
            public DataElement(VisualElement element, Label dataLabel, System.Action<DataElement> onClose)
            {
                Element = element;
                DataLabel = dataLabel;
                var closeBtn = Element.Q<VisualElement>("close-btn");
                closeBtn.AddManipulator(new Clickable(()=>onClose(this)));
            }

            public VisualElement Element { get;}
            public Label DataLabel { get;  }

            public void SetPosition(Vector2 position)
            {
                Element.style.top = position.y;
                Element.style.left = position.x;
            }
            public void SetText(string text)=> DataLabel.text = text;

            
        }
        private readonly Design _design;
        private bool _deleteOnNextUpdate;
        private IStateMachine _stateMachine;
        private VisualElement _content;
        private Node _currentNode;
        private List<string> _actionsExecuted = new List<string>();
        private bool _hasCustomDataFormatter => _dataFormatter != null;
        private ICustomDataTypeFormatter _dataFormatter;
        private Stack<DataElement> _dataElementPool = new Stack<DataElement>(100);
        private List<DataElement> _visibleElements = new List<DataElement>();
        private VisualTreeAsset _templateAsset;
        public DebugDataRenderer(Design design, VisualElement content, EasyStateMachine easyStateMachine)
        {
            _content = content;
            _design = design;
            DebuggerWindow.Instance.EditorEnteredPlayMode += OnEditorEnteredPlayMode;
            DebuggerWindow.Instance.EditorExitedPlayMode += OnEditorExitPlayMode;
            _templateAsset = DebugTemplateFactory.GetDataTemplateAsset();
            for (int i = 0; i < 100; i++)
            {
                _dataElementPool.Push(BuildElement());
            }
            if (Application.isPlaying)
            {
                OnEditorEnteredPlayMode(easyStateMachine);
            }
        }
        private DataElement BuildElement()
        {
            var dataEle = _templateAsset.CloneTree().Q<VisualElement>("data-ele-container");
            var dataLabel = dataEle.Q<Label>();
            return new DataElement(dataEle,dataLabel,OnDataElementClosed);
        }
        private void OnDataElementClosed(DataElement element)
        {
            element.Element.RemoveFromHierarchy();
            _visibleElements.Remove(element);
            _dataElementPool.Push(element);
        }
        private void OnEditorExitPlayMode()
        {
            if (_design != null)
            {
                RemoveDataElements();
            }
        }

        private void OnEditorEnteredPlayMode(EasyStateMachine stateMachineComponent)
        {
            if (stateMachineComponent != null && stateMachineComponent?.StateMachine != null)
            {
                _stateMachine = stateMachineComponent.StateMachine;
                _stateMachine.OnUpdateCycleCompleted += StateMachine_OnUpdateCycleCompleted;
                _stateMachine.OnStateUpdated += StateMachine_OnStateUpdated;
                _stateMachine.OnActionExecuted += StateMachine_OnActionExecuted;
                if (_stateMachine.DataBaseInstance is ICustomDataTypeFormatter)
                {
                    _dataFormatter = (ICustomDataTypeFormatter)_stateMachine.DataBaseInstance;
                }
            }

        }

        private void StateMachine_OnActionExecuted(string nameOfActionExecuted)
        {
            _actionsExecuted.Add(nameOfActionExecuted);
        }

        private void StateMachine_OnStateUpdated(Models.State stateUpdated)
        {
            if (_deleteOnNextUpdate)
            {
                RemoveDataElements();
            }
            var node = _design.Nodes.First(x => x.Id == stateUpdated.Id);
            if (node == _currentNode)
            {
                RemoveDataElements();
                BuildDataElements(node);
            }
            else
            {
                BuildDataElements(node);
            }
            _currentNode = node;
        }

        private void StateMachine_OnUpdateCycleCompleted()
        {
            _deleteOnNextUpdate = true;
        }

        private void BuildDataElements(Node node)
        {
            var dataElement = _dataElementPool.Pop();
            var nodePos = node.Position;
            nodePos.y += 55;
            var relativePos = _design.GetAbsolutePosition(nodePos);

            dataElement.SetPosition(relativePos);
            var dataClone = UnityEngine.Object.Instantiate(_stateMachine.DataBaseInstance);

            string dataJson;
            if (_hasCustomDataFormatter)
            {
                dataJson = _dataFormatter.GetDataAsString();
            }
            else
            {
                dataJson = JsonUtility.ToJson(dataClone, true);
            }



            if (_actionsExecuted.Any())
            {
                dataJson += "\n----------------";
#if UNITY_2021_1_OR_NEWER
                dataJson += "\n<b>Actions Executed<b>";
#else
                dataJson += "\nActions Executed";
#endif
                dataJson += "\n----------------";
                foreach (var action in _actionsExecuted)
                {
#if UNITY_2021_1_OR_NEWER
               
                    dataJson += $"\n<i>{action}</i>";
#else
                    dataJson += $"\n{action}";
#endif
                }
            }
            _actionsExecuted.Clear();
            
            dataElement.SetText(dataJson);             
            _content.Add(dataElement.Element); 
            _visibleElements.Add(dataElement);
        }

        private void RemoveDataElements()
        {
            var elementsToDelete = new List<DataElement>(_visibleElements);
            foreach (var ele in elementsToDelete)
            {
                OnDataElementClosed(ele);
            }
            _deleteOnNextUpdate = false;
        }

        public void Dispose()
        {
            DebuggerWindow.Instance.EditorEnteredPlayMode -= OnEditorEnteredPlayMode;
            DebuggerWindow.Instance.EditorExitedPlayMode -= OnEditorExitPlayMode;
        }
    }
}
