using EasyState.Settings;
using System.IO;

namespace EasyState.Core.Utility
{
    public static class FilePaths
    {
        private static EasyStateSettings _settings = EasyStateSettings.Instance;
        public static readonly string EasyStateFolder = _settings.EasyStateFolder;
        public static readonly string EditorFolder = Path.Combine(EasyStateFolder, "Editor");
        public static readonly string UXMLFolder = Path.Combine(EditorFolder, "UXML");
        public static readonly string EditorImageFolder = Path.Combine(EditorFolder, "Resources", "Images");
        public static readonly string DesignerUXMLPath = Path.Combine(UXMLFolder, "Designer.uxml");
        public static readonly string DesignerUSSPath = Path.Combine(UXMLFolder, "Designer.uss");
        public static readonly string DesignerNodesUSSPath = Path.Combine(UXMLFolder, "NodeStyles.uss");
        public static readonly string DesignerLightUSSPath = Path.Combine(UXMLFolder, "LightSkinStyles.uss");
        public static readonly string DebuggerFolderPath = Path.Combine(EditorFolder, "Debugger");
        public static readonly string DebuggerUXMLPath = Path.Combine(UXMLFolder,"Debugger", "DebuggerWindow.uxml");
        public static string EditorDataFolder
        {
            get
            {
#if UNITY_EDITOR
                if (!File.Exists(_settings.EasyStateDataFolder))
                {
                    Directory.CreateDirectory(_settings.EasyStateDataFolder);
                }
#endif
                return _settings.EasyStateDataFolder;

            }
        }
        public static string Combine(params string[] paths) => Path.Combine(paths);
    }
}
