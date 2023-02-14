using EasyState.Core.Utility;
using System.IO;
using UnityEngine;

namespace EasyState.Settings
{
    public class EasyStateSettings : ScriptableObject
    {
        private static EasyStateSettings _instance;

        public static EasyStateSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = LoadSettings();
                }
                return _instance;
            }
        }
        [Tooltip("Minimum Zoom in the designer")]
        public float MinZoom = .3f;
        [Tooltip("Maximum Zoom inside designer")]
        public float MaxZoom = 1.5f;
        [Tooltip("How sensitive the zoom is inside the designer")]
        public float ZoomStep = .1f;
        [Tooltip("Whether or not the mouse scroll wheel input should be negated in the designer")]
        public bool InvertZoom = false;
        [Tooltip("Should nodes snap to a grid inside the designer?")]
        public bool SnapToGrid = true;
        [Tooltip("Should connection handles snap to a grid inside the designer?")]
        public bool ConnectionsSnapToGrid = false;
        [Tooltip("Size of the grid nodes will snap to inside the designer")]
        public float SnapToGridStep = 25f;
        [Tooltip("A collection of node presets that will populate the conext menu when the designer's background is right clicked.")]
        public NodePresetCollection NodePresetCollection;
        [Tooltip("Settings that effect the appearance and behavior of connections inside the designer.")]
        public ConnectionSettings ConnectionSettings;
        [Tooltip("Get debug log messages when a change has been made to Easy State tracked files.")]
        public bool LogFileChanges = false;
        [Tooltip("Cache states when loading state machines. This improves performance but can cause issues if states should not be treated the same accross systems.")]
        public bool CacheStateSets = false;
        [Tooltip("If true, Easy state will load all designs into an in memory cache for builds, making start up time for state machines faster.")]
        public bool CacheAllDesignsOnBuild = true;
        [Tooltip("If true Easy State will add custom icons to scripts created by the framework to allow for easier recognition")]
        public bool UseCustomIcons = true;
        [Tooltip("Root folder of where the Easy State Asset is located")]
        public string EasyStateFolder = Path.Combine("Assets", "EasyState");
        // Data must be stored in streaming assets to be included in builds
        public string EasyStateDataFolder => Path.Combine(Application.streamingAssetsPath, "EasyStateData");
        [Tooltip("Color of the designer's background")]
        public Color BackgroundColor = EditorColors.BackgroundColor;
        [Tooltip("Color of the designer's toolbar background")]
        public Color ToolbarColor = EditorColors.ToolbarColor;
        [Tooltip("Custom icon for Data Type scripts")]
        public Texture2D DataTypeIcon;
        [Tooltip("Custom icon for Action scripts")]
        public Texture2D ActionIcon;
        [Tooltip("Custom icon for Condition scripts")]
        public Texture2D ConditionIcon;
        [Tooltip("Custom icon for Evaluator scripts")]
        public Texture2D EvaluatorIcon;
        [Tooltip("Custom icon for Event Handler scripts")]
        public Texture2D EventHandlerIcon;
        [Tooltip("Custom icon for State Machine scripts")]
        public Texture2D StateMachineIcon;
        /// <summary>
        /// This is used by Easy State to track when an event handler has been created and needs to be added to a gameobject
        /// </summary>
        [HideInInspector]
        public ComponentAdded ComponentAddedData = new ComponentAdded();
        /// <summary>
        /// This is used by Easy State to track when a new Easy State script was created and needs to be given a custom icon if <see cref="UseCustomIcons"/> 
        /// is set to true.
        /// </summary>
        [HideInInspector]
        public EasyComponentCreationDetails ComponentCreationDetails = new EasyComponentCreationDetails();
        /// <summary>
        /// Resets the settings object to the default settings
        /// </summary>
        public void Reset()
        {

            NodePresetCollection = new NodePresetCollection();
            NodePresetCollection.Reset();
            ConnectionSettings = new ConnectionSettings();
            ComponentAddedData = new ComponentAdded();
            ComponentCreationDetails = new EasyComponentCreationDetails();
            MinZoom = .3f;
            MaxZoom = 1.5f;
            ZoomStep = .1f;
            UseCustomIcons = true;
            InvertZoom = false;
            SnapToGrid = true;
            ConnectionsSnapToGrid = false;
            SnapToGridStep = 25f;
            LogFileChanges = false;
            EasyStateFolder = Path.Combine("Assets", "EasyState");
            BackgroundColor = EditorColors.BackgroundColor;
            ToolbarColor = EditorColors.ToolbarColor;
#if UNITY_EDITOR
           ReloadIcons();
#endif
        }
#if UNITY_EDITOR
        public void ReloadIcons()
        {
            string iconFolder = Path.Combine(EasyStateFolder, "Editor", "Resources", "Images");
            DataTypeIcon = UnityEditor.AssetDatabase.LoadAssetAtPath<Texture2D>(Path.Combine(iconFolder, "DataIcon.png"));
            ActionIcon = UnityEditor.AssetDatabase.LoadAssetAtPath<Texture2D>(Path.Combine(iconFolder, "ActionIcon.png"));
            ConditionIcon = UnityEditor.AssetDatabase.LoadAssetAtPath<Texture2D>(Path.Combine(iconFolder, "ConditionIcon.png"));
            EvaluatorIcon = UnityEditor.AssetDatabase.LoadAssetAtPath<Texture2D>(Path.Combine(iconFolder, "EvalIcon.png"));
            EventHandlerIcon = UnityEditor.AssetDatabase.LoadAssetAtPath<Texture2D>(Path.Combine(iconFolder, "EventHandlerIcon.png"));
            StateMachineIcon = UnityEditor.AssetDatabase.LoadAssetAtPath<Texture2D>(Path.Combine(iconFolder, "StateMachineIcon.png"));
        } 
#endif
        /// <summary>
        /// Loads the singleton reference of the Easy State settings object
        /// </summary>
        /// <returns></returns>
        private static EasyStateSettings LoadSettings()
        {
            var settings = Resources.Load<EasyStateSettings>("Settings");
            if (settings is null)
            {
                Debug.Log("Could not find Easy State Settings object");
#if UNITY_EDITOR
                var defaultResourceFolderPath = Path.Combine("Assets", "EasyState", "Resources");
                if (!Directory.Exists(defaultResourceFolderPath))
                {
                    Directory.CreateDirectory(defaultResourceFolderPath);
                }
                Debug.Log($"Creating new settings object in the { Path.Combine("Assets", "EasyState", "Resources")} folder.");
                settings = CreateInstance<EasyStateSettings>();
                settings.Reset();
                UnityEditor.AssetDatabase.CreateAsset(settings, Path.Combine(defaultResourceFolderPath, "Settings.asset"));
#endif
            }

            return settings;
        }
    }
}