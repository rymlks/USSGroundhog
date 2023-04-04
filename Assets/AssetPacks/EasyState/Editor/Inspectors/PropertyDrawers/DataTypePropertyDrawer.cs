using EasyState.DataModels;
using EasyState.Models;
using System;
using UnityEditor;
using UnityEngine;

namespace EasyState.Inspectors.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(DataTypeAttribute), true)]
    public class DataTypePropertyDrawer : PropertyDrawer
    {
        private const int buttonWidth = 86;
        private DataTypeModel _dataType;


        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float totalHeight = EditorGUIUtility.singleLineHeight;
            if (property.objectReferenceValue == null || !AreAnySubPropertiesVisible(property))
            {
                return totalHeight;
            }
            if (property.isExpanded)
            {
                var data = property.objectReferenceValue as ScriptableObject;
                if (data == null) return EditorGUIUtility.singleLineHeight;
                SerializedObject serializedObject = new SerializedObject(data);
                SerializedProperty prop = serializedObject.GetIterator();
                if (prop.NextVisible(true))
                {
                    do
                    {
                        if (prop.name == "m_Script") continue;
                        var subProp = serializedObject.FindProperty(prop.name);
                        float height = EditorGUI.GetPropertyHeight(subProp, null, true) + EditorGUIUtility.standardVerticalSpacing;
                        totalHeight += height;
                    }
                    while (prop.NextVisible(false));
                }
                // Add a tiny bit of height if open for the background
                totalHeight += EditorGUIUtility.standardVerticalSpacing;
            }
            return totalHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            Rect propertyRect;
            var guiContent = new GUIContent(property.displayName);
            var foldoutRect = new Rect(position.x + 10, position.y, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight);
            if (property.objectReferenceValue != null && AreAnySubPropertiesVisible(property))
            {
                property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, guiContent, true);
            }
            else
            {
                foldoutRect.x += 12;
                EditorGUI.Foldout(foldoutRect, property.isExpanded, guiContent, true, EditorStyles.label);
            }
            DataTypeAttribute attr = attribute as DataTypeAttribute;
            var indentedPosition = EditorGUI.IndentedRect(position);
            var indentOffset = indentedPosition.x - position.x;
            propertyRect = new Rect(position.x + (EditorGUIUtility.labelWidth - indentOffset), position.y, position.width - (EditorGUIUtility.labelWidth - indentOffset), EditorGUIUtility.singleLineHeight);

            if (!attr.ReadOnly)
            {
                propertyRect.width -= buttonWidth;
            }

            if (!attr.ReadOnly)
            {
                GUI.enabled = !Application.isPlaying;
                EditorGUI.PropertyField(propertyRect, property, GUIContent.none);
                GUI.enabled = true;
            }
            // GUI.enabled = true;
            if (GUI.changed) property.serializedObject.ApplyModifiedProperties();

            var buttonRect = new Rect(position.x + position.width - buttonWidth, position.y, buttonWidth, EditorGUIUtility.singleLineHeight);

            if (property.propertyType == SerializedPropertyType.ObjectReference && property.objectReferenceValue != null)
            {
                var data = (ScriptableObject)property.objectReferenceValue;

                if (property.isExpanded)
                {
                    // Draw a background that shows us clearly which fields are part of the ScriptableObject
                    GUI.Box(new Rect(0, position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing - 1, Screen.width, position.height - EditorGUIUtility.singleLineHeight - EditorGUIUtility.standardVerticalSpacing), "");

                    EditorGUI.indentLevel++;
                    SerializedObject serializedObject = new SerializedObject(data);

                    // Iterate over all the values and draw them
                    SerializedProperty prop = serializedObject.GetIterator();
                    float y = position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    if (prop.NextVisible(true))
                    {
                        do
                        {
                            // Don't bother drawing the class file
                            if (prop.name == "m_Script") continue;
                            float height = EditorGUI.GetPropertyHeight(prop, new GUIContent(prop.displayName), true);
                            EditorGUI.PropertyField(new Rect(position.x, y, position.width - buttonWidth, height), prop, true);

                            y += height + EditorGUIUtility.standardVerticalSpacing;
                        }
                        while (prop.NextVisible(false));
                    }
                    if (GUI.changed)

                        serializedObject.ApplyModifiedProperties();

                    EditorGUI.indentLevel--;
                }
            }
            if (!attr.ReadOnly && !Application.isPlaying)
            {
                if (GUI.Button(buttonRect, "Create New"))
                {
                    CreateAssetWithSavePrompt(fieldInfo.FieldType, "Assets/");
                }
            }

            property.serializedObject.ApplyModifiedProperties();
            try
            {
                EditorGUI.EndProperty();
            }
            catch (InvalidOperationException)
            {
                //this error started getting thrown in Unity 2020.1.2
            }
        }

        private static bool AreAnySubPropertiesVisible(SerializedProperty property)
        {
            var data = (ScriptableObject)property.objectReferenceValue;
            SerializedObject serializedObject = new SerializedObject(data);
            SerializedProperty prop = serializedObject.GetIterator();
            while (prop.NextVisible(true))
            {
                if (prop.name == "m_Script") continue;
                return true; //if theres any visible property other than m_script
            }
            return false;
        }

        // Creates a new ScriptableObject via the default Save File panel
        private static ScriptableObject CreateAssetWithSavePrompt(Type type, string path)
        {
            path = EditorUtility.SaveFilePanelInProject("Save ScriptableObject", type.Name + ".asset", "asset", "Enter a file name for the ScriptableObject.", path);
            if (path == "") return null;
            ScriptableObject asset = ScriptableObject.CreateInstance(type);
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
            EditorGUIUtility.PingObject(asset);
            return asset;
        }
    }
}