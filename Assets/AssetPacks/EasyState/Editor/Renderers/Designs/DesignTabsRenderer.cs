using EasyState.Core.Models;
using EasyState.Editor.Templates;
using EasyState.Editor.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;

namespace EasyState.Editor.Renderers.Designs
{
    public class DesignTabsRenderer : IDisposable
    {
        public DesignRenderer SelectedDesignRenderer { get; private set; }
        public ContextMenuRenderer ContextMenuRenderer;
        private const string MENU_CONTAINER_ID = "menu-container";
        private readonly VisualElement _contentContainer;
        private readonly VisualElement _rootElement;
        private readonly TabContainer _tabContainer;
        private readonly List<DesignRenderer> _tabs = new List<DesignRenderer>();
        private VisualElement _menuContainer;
        private ShortcutManager _shortcutManager;
        public DesignTabsRenderer(TabContainer tabContainer, VisualElement contentContainer, VisualElement root)
        {
            _rootElement = root;
            _tabContainer = tabContainer;
            tabContainer.DesignAdded += TabContainer_DesignAdded;
            _tabContainer.DesignClosed += TabContainer_DesignClosed;
            _contentContainer = contentContainer;
            _tabContainer.TabChanged += TabContainer_TabChanged;
            _menuContainer = root.Q<VisualElement>(MENU_CONTAINER_ID);
            _shortcutManager = new ShortcutManager(root);
        }

        public void Dispose()
        {
            foreach (DesignRenderer renderer in _tabs)
            {
                renderer.Dispose();
            }
            _tabContainer.DesignAdded -= TabContainer_DesignAdded;
            _tabContainer.DesignClosed -= TabContainer_DesignClosed;
            _shortcutManager.Dispose();
        }

        private void AddDesign(Design design)
        {
            VisualElement tabTemplate = TemplateFactory.CreateTabContent();
            DesignRenderer newTab = new DesignRenderer(design, tabTemplate, _rootElement);
            _tabs.Add(newTab);
            _contentContainer.Insert(0, newTab);

            SetNewDesign(design);
        }

        private void HidePreviousDesign()
        {
            if (SelectedDesignRenderer != null)
            {
                //Unsub context menu requests for this design
                if (ContextMenuRenderer != null)
                {
                    ContextMenuRenderer.Dispose();
                    ContextMenuRenderer = null;
                }
                SelectedDesignRenderer.Hide();
            }
        }

        private void SetNewDesign(Design newDesign)
        {
            HidePreviousDesign();
            SelectedDesignRenderer = _tabs.First(x => x.Model == newDesign);
            SelectedDesignRenderer.Show();
            _shortcutManager.CurrenDesign = newDesign;
            //Sub for context menu events for this design
            ContextMenuRenderer = new ContextMenuRenderer(_menuContainer, SelectedDesignRenderer);
        }

        private void TabContainer_DesignAdded(Design design) => AddDesign(design);

        private void TabContainer_DesignClosed(Design design)
        {
            DesignRenderer renderer = _tabs.First(x => x.Model == design);
            _contentContainer.Remove(renderer);
            renderer.Dispose();
            if (ContextMenuRenderer != null)
            {
                ContextMenuRenderer.Dispose();
                ContextMenuRenderer = null;
            }
            _tabs.Remove(renderer);
            _shortcutManager.CurrenDesign = null;
        }

        private void TabContainer_TabChanged()
        {
            //if selected tab not a design it must be the context wizard
            if (!(_tabContainer.SelectedTab is DesignTab))
            {
                if (SelectedDesignRenderer != null)
                {
                    HidePreviousDesign();
                    SelectedDesignRenderer = null;
                }
                return;
            }
            //selected tab must be a design
            var design = ((DesignTab)_tabContainer.SelectedTab).Design;
            //selected design has not changed
            if (SelectedDesignRenderer?.Model == design)
            {
                return;
            }
            SetNewDesign(design);
        }
    }
}