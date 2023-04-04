using EasyState.Core.Utility;
using EasyState.Data;
using EasyState.Editor.Debugger.Renderers;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;

namespace EasyState.Editor.Debugger
{
    internal class DebuggerWindowBootstrapper : IDisposable
    {
        private VisualElement _root;
        private List<IDisposable> _disposables = new List<IDisposable>();
        private DebugStateMachineSelectionRenderer _selectionRenderer;
        public DebuggerWindowBootstrapper(VisualElement root)
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
            RebuildDesignWindow();
            Initialize();
        }

        private void Initialize()
        {
            _selectionRenderer = new DebugStateMachineSelectionRenderer(_root);
            _disposables.Add(_selectionRenderer);
        }

        public void SetStateMachine(EasyStateMachine stateMachineInstance)
        {
            var designData = new DesignDatabase(stateMachineInstance.Behavior.DesignId).Load();
            var content = _root.Q<VisualElement>("content");
            var design = DesignLoader.Load(designData);
            _disposables.Add(new DebugDesignRenderer(design, _root));
            _disposables.Add(new DebugDataRenderer(design, content, stateMachineInstance));
            new DebugToolBarRenderer(design, _root.Q<VisualElement>("toolbar"));
            _selectionRenderer.Hide();
        }

        private void RebuildDesignWindow()
        {
            _root.Clear();
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(FilePaths.DebuggerUXMLPath);
            visualTree.CloneTree(_root);
        }
        public void Dispose()
        {
            _disposables.ForEach(x => x.Dispose());
        }
    }
}
