using EasyState.Core.Utility;
using System.IO;
using UnityEditor;
using UnityEngine.UIElements;

namespace EasyState.Editor.Debugger
{
    internal static class DebugTemplateFactory
    {
        private static readonly string _templateFolder = Path.Combine(FilePaths.UXMLFolder, "Debugger");

        public static VisualElement GetSelectionPanelTemplate() => LoadDebugElement("SelectionPanel.uxml").Q<VisualElement>("select-container");
        public static VisualTreeAsset GetDataTemplateAsset() => AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(Path.Combine(_templateFolder, "DataElementTemplate.uxml"));
        private static VisualElement LoadDebugElement(string elementName)
        {
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(Path.Combine(_templateFolder, elementName));
            return visualTree.CloneTree();
        }
    }
}
