using EasyState.DataModels;
using EasyState.Core.Models;
using EasyState.Data;
using EasyState.Editor.Templates;
using EasyState.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using EasyState.Editor.Utility;

namespace EasyState.Editor.Renderers.Designs
{
    public class TabContainer : IDisposable
    {
        public event Action AddDesignRequest;

        public event Action<Design> DesignAdded;

        public event Action<Design> DesignClosed;

        public event Action TabChanged;

        public List<DesignTab> DesignTabs { get; private set; } = new List<DesignTab>();
        public DesignTab SelectedTab { get; private set; }
        private readonly VisualElement _addDesignBtn;
        private readonly VisualElement _tabsContainer;
        private bool _loaderOpen;

        public TabContainer(VisualElement sideBar)
        {
            _tabsContainer = sideBar.Q<VisualElement>("tabs");
            sideBar.style.backgroundColor = EasyStateSettings.Instance.ToolbarColor;
            _addDesignBtn = sideBar.Q<VisualElement>("add-tab");
            _addDesignBtn.RegisterCallback<MouseDownEvent>(OnAddDesignRequested);
        }

        public void Dispose()
        {
            _addDesignBtn.UnregisterCallback<MouseDownEvent>(OnAddDesignRequested);
            if (DesignTabs != null)
            {
                DesignTabs.ForEach(x => x.Dispose());
            }
        }

        public void OnAddDesignRequestCompleted() => _loaderOpen = false;

        public void OnDesignAdded(Design design)
        {
            VisualElement template = TemplateFactory.CreateTab().Q<VisualElement>("design-tab");
            DesignTab tab = new DesignTab(template, this, design, DesignTabs.Count);
            design.JumpToDesign += JumpToDesign;
            DesignTabs.Add(tab);
            _tabsContainer.Add(tab.TabElement);
            DesignAdded?.Invoke(design);
            OnTabClicked(tab);
        }

        private void JumpToDesign(string designID, string nodeID)
        {
            var openDesignTab = DesignTabs.FirstOrDefault(x => x.Design.Id == designID);
            if (openDesignTab != null)
            {
                OnTabClicked(openDesignTab);

                var selectedNode = openDesignTab.Design.Nodes.FirstOrDefault(x => x.Id == nodeID);
                if (selectedNode != null)
                {
                    openDesignTab.Design.SelectModel(selectedNode);
                    openDesignTab.Design.OnDetailsPanelRequested(selectedNode);
                }
            }
            else
            {
                var design = DesignLoader.Load(designID);
                OnDesignAdded(design);
                var selectedNode = design.Nodes.FirstOrDefault(x => x.Id == nodeID);
                if (selectedNode != null)
                {
                    design.SelectModel(selectedNode);
                    design.OnDetailsPanelRequested(selectedNode);
                }
            }

        }

        public void OnDesignClosed(DesignTab tab, bool showSaveWarning = true)
        {
            if (showSaveWarning)
            {
                var previousDesignData = new DesignDatabase(tab.Design.Id).Load();
                bool hasChanges = !tab.Design.Serialize().DataEqualTo(previousDesignData);
                if (hasChanges)
                {
                    if (EditorUtility.DisplayDialog("Save changes?", "Do you want to save changes before closing?", "Yes", "No"))
                    {
                        tab.Design.SaveChanges();
                    }
                }
            }
            int selectIndex = -1;

            if (tab.IsSelected)
            {
                int index = DesignTabs.IndexOf(tab);
                selectIndex = Mathf.Max(0, index - 1);
            }
            DesignTabs.Remove(tab);
            _tabsContainer.Remove(tab.TabElement);
            tab.Dispose();
            DesignClosed?.Invoke(tab.Design);
            tab.Design.JumpToDesign -= JumpToDesign;
            if (SelectedTab.Index > tab.Index)
            {
                //tab above selected tab closed
            }
            else if (DesignTabs.Count > 0 && selectIndex != -1)
            {
                var newSelectedTab = DesignTabs[selectIndex];
                OnTabClicked(newSelectedTab);
            }
            else
            {
                SelectedTab = null;
                TabChanged?.Invoke();
            }
        }

        public void OnDesignDeleted(DesignDataShort design)
        {
            var openTab = DesignTabs.FirstOrDefault(x => x.Design.Id == design.Id);
            if (openTab != null)
            {
                OnDesignClosed(openTab, false);
            }
        }

        public void OnTabClicked(DesignTab clickedTab)
        {
            if (SelectedTab != null)
            {
                SelectedTab.DeSelect();
            }
            SelectedTab = clickedTab;
            clickedTab.Select();
            TabChanged?.Invoke();
        }

        public DesignerWindowState SerializeState()
        {
            return new DesignerWindowState
            {
                OpenDesigns = DesignTabs.Select(x => x.Design.Serialize()).ToList(),
                SelectedTab = SelectedTab?.Index ?? -99,
                LoaderOpen = _loaderOpen
            };
        }

        public void SetState(DesignerWindowState state)
        {
            foreach (var tab in state.OpenDesigns)
            {
                var design = DesignLoader.Load(tab);
                OnDesignAdded(design);
            }
            var selectedDesign = DesignTabs.FirstOrDefault(x => x.Index == state.SelectedTab);
            if (selectedDesign != null)
            {
                OnTabClicked(selectedDesign);
            }

            if (state.LoaderOpen)
            {
                AddDesignRequest?.Invoke();
            }
        }

        private void OnAddDesignRequested(MouseDownEvent evt)
        {
            _loaderOpen = true;
            AddDesignRequest?.Invoke();
        }
    }
}