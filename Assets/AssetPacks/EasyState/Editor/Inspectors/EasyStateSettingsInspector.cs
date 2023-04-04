using EasyState.Settings;
using UnityEditor;
using UnityEngine;

namespace EasyState.Editor.Insepctors
{
    [CustomEditor(typeof(EasyStateSettings))]
    public class EasyStateSettingsInspector : UnityEditor.Editor
    {
        private EasyStateSettings _settingsObject;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Reset to Default"))
            {
                _settingsObject.Reset();
                EditorUtility.SetDirty(_settingsObject);
            }
            if (GUILayout.Button("Reload Script Icons"))
            {
                _settingsObject.ReloadIcons();
                EditorUtility.SetDirty(_settingsObject);
            }
        }

        private void Awake()
        {
            _settingsObject = target as EasyStateSettings;
        }
    }
}