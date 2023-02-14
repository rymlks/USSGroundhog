using EasyState.Core.Models;
using EasyState.Data;
using EasyState.Editor.Utility;
using EasyState.Settings;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace EasyState.Editor.Renderers.Designs
{
    public class ToolBarRenderer : IDisposable
    {
        private Label _dataTypeLabel;
        private Label _designLabel;
        private TabContainer _tabContainer;
        private TextField _designName;
        private VisualElement _menu;
        private bool _showMenu = false;
        private string _cachedDesignName;
        private Design _currentDesign => _tabContainer.SelectedTab.Design;
        public ToolBarRenderer(TabContainer tabContainer, VisualElement toolbar)
        {
            _tabContainer = tabContainer;
            _tabContainer.TabChanged += TabContainer_TabChanged;
            toolbar.style.backgroundColor = EasyStateSettings.Instance.ToolbarColor;
            _dataTypeLabel = toolbar.Q<Label>("data-type-name");
            _designLabel = toolbar.Q<Label>("design-name");
            _menu = toolbar.Q<VisualElement>("menu");
            var imGUI = new IMGUIContainer();
            imGUI.onGUIHandler = DrawMenu;
            _designName = toolbar.Q<TextField>("design-name-input");
            _designName.isDelayed = true;
            _designName.RegisterValueChangedCallback(x =>
            {
                var design = ((DesignTab)_tabContainer.SelectedTab).Design;
                if (string.IsNullOrEmpty(x.newValue))
                {
                    design.Name = _cachedDesignName;
                }
                else
                {
                    design.Name = x.newValue;
                }
                design.SaveChanges();
                _tabContainer.SelectedTab.SetText(design.Name);
                _designName.style.display = DisplayStyle.None;
                _designLabel.text = "Design : " + design.Name; 

               //OnRebuildDesigner();

            });
            var clickable = new Clickable(() =>
            {

                _designLabel.text = "Design :";
                _designName.style.display = DisplayStyle.Flex;
                _designName.SetValueWithoutNotify(_cachedDesignName);
            });
            clickable.activators.Clear();
            clickable.activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse, clickCount = 1 });

            _designLabel.AddManipulator(clickable);
            _menu.RegisterCallback<MouseDownEvent>(e =>
            {
                _showMenu = true;
                imGUI.MarkDirtyRepaint();

            }

            );
            _menu.Add(imGUI);

            TabContainer_TabChanged();
        }

        public void Dispose()
        {
            _tabContainer.TabChanged -= TabContainer_TabChanged;
        }
        private void DrawMenu()
        {
            if (_showMenu)
            {
                GenericMenu menu = new GenericMenu();
                menu.AddItem(new GUIContent("Save (Ctrl+S)"), false, OnSaveClicked);
                menu.AddItem(new GUIContent("Duplicate Design"), false, OnDuplicateDesign);
                menu.AddItem(new GUIContent("Rebuild Designer (Ctrl+R)"), false, OnRebuildDesigner);
                menu.AddItem(new GUIContent("Validate Design (Ctrl+V)"), false, () => _currentDesign.OnValidateDesign());
                _showMenu = false;

                menu.ShowAsContext();
            }
        }

        private void OnSaveClicked()
        {
            if (_tabContainer.SelectedTab == null)
            {
                return;
            }
            _tabContainer.SelectedTab.Design.SaveChanges();
        }
        private void OnDuplicateDesign()
        {
            var designData = _currentDesign.Serialize();
            designData.Id = Guid.NewGuid().ToString();
            designData.Name += "_Copy";
            designData = DesignExetensions.RandomizeStateIDs(designData);
            var db = new DesignDatabase(designData.Id);
            db.Save(designData);

            var duplicatedDesign = DesignLoader.Load(designData.Id);

            _tabContainer.OnDesignAdded(duplicatedDesign);

            duplicatedDesign.OnShowToast("Design duplicated", 2500);
        }
        private void OnRebuildDesigner()
        {
            var window = DesignerWindow.Instance;
            window?.OnCreateGUI();
        }
        private void TabContainer_TabChanged()
        {
            if (_tabContainer.SelectedTab == null)
            {
                _designLabel.text = "";
                _dataTypeLabel.text = "";
                return;
            }
            _designName.style.display = DisplayStyle.None;
            _designLabel.text = $"Design : {_currentDesign.Name}";
            _dataTypeLabel.text = $"Data Type : {_currentDesign.DataType.Name}";
            _cachedDesignName = _currentDesign.Name;
            _designName.SetValueWithoutNotify(_cachedDesignName);

        }
    }
}