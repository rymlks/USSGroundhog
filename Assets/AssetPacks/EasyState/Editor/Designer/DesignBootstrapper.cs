using EasyState.Core.Utility;
using EasyState.Data;
using EasyState.Editor.Renderers.Designs;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;

namespace EasyState.Editor.Designer
{
    public class DesignBootstrapper : IDisposable
    {  
        private List<IDisposable> _disposables;
        private VisualElement _root;
        private TabContainer _tabContainer;

        public DesignBootstrapper(VisualElement root)
        {
            _root = root;
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(FilePaths.DesignerUSSPath);
            var nodeStyleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(FilePaths.DesignerNodesUSSPath);
            _root.styleSheets.Add(styleSheet);
            _root.styleSheets.Add(nodeStyleSheet);
            if (!EditorGUIUtility.isProSkin)
            {
                var lightStyle = AssetDatabase.LoadAssetAtPath<StyleSheet>(FilePaths.DesignerLightUSSPath);
                _root.styleSheets.Add(lightStyle);
            }
            Initialize();
        }

        public void Dispose()
        {           
            if (_disposables != null)
            {
                _disposables.ForEach(x => x.Dispose());
            }
        }

        public void SaveDesignState()
        {
            if (_tabContainer != null)
            {
                var state = _tabContainer.SerializeState();
                var windowDatabase = new DesignWindowDatabase();
                windowDatabase.Save(state);
            }
        }

        private void Initialize()
        {
            RebuildDesignWindow();
            var sideBar = _root.Q<VisualElement>("side-bar");
            _tabContainer = new TabContainer(sideBar);
            _tabContainer.DesignClosed += x => SaveDesignState();
            var toastRenderer = new ToastRenderer(_root.Q<VisualElement>("toast"), _tabContainer);
            DesignLoaderRenderer designLoader = new DesignLoaderRenderer(_root, _tabContainer);
            _disposables = new List<IDisposable>
            {
                _tabContainer,
                 designLoader,
                new DesignTabsRenderer(_tabContainer, _root.Q<VisualElement>("content"), _root),
                new ToolBarRenderer(_tabContainer, _root.Q<VisualElement>("toolbar")),
                
              
            };
            var windowDatabase = new DesignWindowDatabase();
            var state = windowDatabase.Load();

            _tabContainer.SetState(state);
            if(_tabContainer.DesignTabs.Count == 0)
            {
                designLoader.Show();
            }
        }

        private void RebuildDesignWindow()
        {
            _root.Clear();
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(FilePaths.DesignerUXMLPath);
            visualTree.CloneTree(_root);
        }
    }
}