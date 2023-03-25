using EasyState.Core.Utility;
using System.IO;
using UnityEditor;
using UnityEngine.UIElements;

namespace EasyState.Editor.Templates
{
    public static class TemplateFactory
    {
        private static readonly string _templateFolder = FilePaths.UXMLFolder;
        private static readonly string _detailTemplateFolder = Path.Combine(_templateFolder, "DetailTemplates");
        private static readonly string _nodeDetailTemplateFolder = Path.Combine(_detailTemplateFolder, "Node");

        public static VisualElement CreateDesignGridRowTemplate() => LoadDetailElement("DesignGridRow.uxml").Q<VisualElement>("row");

        public static VisualElement CreateDesignLoaderTemplate() => LoadElement("DesignLoader.uxml").Q<VisualElement>("loader-container");
        public static VisualElement CreateDebugTemplate() => LoadElement("DebugPanel.uxml").Q<VisualElement>("container");

        public static VisualElement CreateDetailTemplate(string prefix) => LoadDetailElement(prefix + "DetailTemplate.uxml").Q<VisualElement>("content");

        public static VisualElement CreateGroupTemplate() => LoadElement("GroupTemplate.uxml").Q<VisualElement>("group-container");

        public static VisualElement CreateNode() => LoadElement("NodeTemplate.uxml");

        public static VisualElement CreateNodeSettings() => LoadNodeDetailElement("SettingsTemplate.uxml");

        public static VisualElement CreateNoteTemplate() => LoadElement("NoteTemplate.uxml").Q<VisualElement>("note-container");

        public static VisualElement CreateTab() => LoadElement("TabTemplate.uxml");

        public static VisualElement CreateTabContent() => LoadElement("TabContentTemplate.uxml").Q<VisualElement>("tab-content");

        public static VisualElement CreateValidationResultTemplate() => LoadElement("DesignValidatorWindow.uxml").Q<VisualElement>("container");

        public static VisualTreeAsset GetToastMessageTemplate()
        {
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(Path.Combine(_templateFolder, "ToastMessage.uxml"));
            return visualTree;
        }

        private static VisualElement LoadDetailElement(string elementName)
        {
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(Path.Combine(_detailTemplateFolder, elementName));
            return visualTree.CloneTree();
        }

        private static VisualElement LoadElement(string elementName)
        {
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(Path.Combine(_templateFolder, elementName));
            return visualTree.CloneTree();
        }

        private static VisualElement LoadNodeDetailElement(string elementName)
        {
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(Path.Combine(_nodeDetailTemplateFolder, elementName));
            return visualTree.CloneTree();
        }
    }

}