using EasyState.Core.Models;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace EasyState.Editor.Debugger.Renderers
{
    internal class DebugToolBarRenderer
    {

        private readonly Label _dataTypeLabel;
        private readonly Label _designLabel;
        private VisualElement _menu;
        private bool _showMenu = false;
        public DebugToolBarRenderer(Design design, VisualElement toolbar)
        {

            _dataTypeLabel = toolbar.Q<Label>("data-type-name");
            _dataTypeLabel.tooltip = "Data type name";
            _designLabel = toolbar.Q<Label>("design-name");
            _designLabel.tooltip = "Design name";
            _dataTypeLabel.text = design.DataType.Name;
            _designLabel.text = design.Name;
            _menu = toolbar.Q<VisualElement>("menu");
            var imGUI = new IMGUIContainer();
            imGUI.onGUIHandler = DrawMenu;

            _menu.RegisterCallback<MouseDownEvent>(e =>
            {
                _showMenu = true;
                imGUI.MarkDirtyRepaint();

            });
            _menu.Add(imGUI);


        }
        private void DrawMenu()
        {
            if (_showMenu)
            {
                GenericMenu menu = new GenericMenu();
                menu.AddItem(new GUIContent("Change Design"), false, OnChangeDesignClicked);
                _showMenu = false;

                menu.ShowAsContext();
            }
        }

        private void OnChangeDesignClicked()
        {

            DebuggerWindow.Instance.OnChangeDesign();
        }
    }
}
