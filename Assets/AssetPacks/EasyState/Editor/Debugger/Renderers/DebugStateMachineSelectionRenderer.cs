using System;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace EasyState.Editor.Debugger.Renderers
{
    public class DebugStateMachineSelectionRenderer : IDisposable
    {
        private readonly VisualElement _panel;
        private ObjectField _stateMachineObject;
        private Label _errorMessage;
        private Label _submitButton;
        private EasyStateMachine _selectedStateMachine;
        public DebugStateMachineSelectionRenderer(VisualElement root)
        {
            _panel = DebugTemplateFactory.GetSelectionPanelTemplate();
            root.Add(_panel);
            _stateMachineObject = _panel.Q<ObjectField>("state-machine");
            _stateMachineObject.objectType = typeof(EasyStateMachine);
            _errorMessage = _panel.Q<Label>("error-message");           
            _submitButton = _panel.Q<Label>("submit");
            _submitButton.AddManipulator(new Clickable(OnSubmit));
            _stateMachineObject.RegisterValueChangedCallback(x =>
            {
                if (x.newValue != null)
                {
                    var stateMachine = (EasyStateMachine)x.newValue;

                    _selectedStateMachine = stateMachine;


                }


            });
        }
        private void OnSubmit()
        {
            _errorMessage.text = "";

            if (_selectedStateMachine == null)
            {
                _errorMessage.text = "Select a state machine";
                return;
            }
            if (_selectedStateMachine.Behavior == null)
            {
                _errorMessage.text = "This state machine does not have a design associated with it, therefore it can not be debugged.";
                return;
            }
            DebuggerWindow.Instance.OnStateMachineConfirmed(_selectedStateMachine);
        }
        public void Hide()
        {
            _panel.style.display = DisplayStyle.None;
        }
        public void Show()
        {
            _panel.style.display = DisplayStyle.Flex;

        }
        public void Dispose()
        {
        }
    }

}