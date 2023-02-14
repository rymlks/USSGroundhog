using EasyState.Editor.Utility;
using EasyState.Models;
using EasyState.Settings;
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace EasyState.Editor.Inspectors.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(Models.EasyStateEventHandlerAttribute), true)]
    public class EventHandlerPropertyDrawer : PropertyDrawer
    {
        private const int buttonWidth = 86;
        private Type _eventHandlerType;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

            if (_eventHandlerType == null)
            {
                var dataType = fieldInfo.DeclaringType.GenericTypeArguments[0];
                _eventHandlerType = typeof(Core.EventHandler<>).MakeGenericType(dataType);
            }
            EditorGUI.BeginProperty(position, label, property);
            var guiContent = new GUIContent("Event Handler");
            Rect propertyRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            propertyRect.width -= buttonWidth;
            property.objectReferenceValue = EditorGUI.ObjectField(propertyRect, guiContent, property.objectReferenceValue, _eventHandlerType, true);

            if (GUI.changed) property.serializedObject.ApplyModifiedProperties();

            var buttonRect = new Rect(position.x + position.width - buttonWidth, position.y, buttonWidth, EditorGUIUtility.singleLineHeight);

            if (GUI.Button(buttonRect, "Create New"))
            {

                CreateAssetWithSavePrompt(_eventHandlerType, property.serializedObject.targetObject);

            }
            try
            {
                EditorGUI.EndProperty();
            }
            catch (InvalidOperationException)
            {
                //catching InvalidOperationException: Stack empty. that shows up randomly 
            }

        }
        // Creates a new ScriptableObject via the default Save File panel
        private static UnityEngine.Object CreateAssetWithSavePrompt(Type type, UnityEngine.Object target)
        {
            Type dataType = type.GenericTypeArguments[0];
            string path = EditorUtility.SaveFilePanelInProject("Create Event Handler", dataType.Name + "EventHandler.cs", "cs", "Enter a file name for the Event Handler.", "Assets/");
            if (path == "") return null;
            string fileName = Path.GetFileNameWithoutExtension(path);
            string dataTypeNamespace = dataType.Namespace is null ? string.Empty : dataType.Namespace;
            var template = ScriptGenerator.GetEventHandlerTemplate(fileName, dataType.Name, dataTypeNamespace);
            File.WriteAllText(path, template);
            EasyStateSettings.Instance.ComponentCreationDetails.NewEventHandlerFilePath = path;
            AssetDatabase.ImportAsset(path);
            var componentData = EasyStateSettings.Instance.ComponentAddedData;
            componentData.ChangeWaiting = true;
            componentData.PathToComponent = path;
            componentData.TargetInstanceID = target.GetInstanceID();

            return null;


        }


    }
    public static class ComponentAdder
    {
        [DidReloadScripts]
        private static void SetObjectReference()
        {
            var componentData = EasyStateSettings.Instance.ComponentAddedData;
            if (componentData.ChangeWaiting)
            {
                var gb = Selection.activeGameObject;
                if (gb == null)
                {
                    Debug.LogWarning("No gameObject is selected and therefore an event handler could not be added and must be added manually");

                }
                else
                {
                    MonoScript script = AssetDatabase.LoadAssetAtPath<MonoScript>(componentData.PathToComponent);
                    Type handlerType = script.GetClass();
                    Component component = gb.AddComponent(handlerType);
                    EasyStateMachine machine = gb.GetComponents<EasyStateMachine>().First(x => x.GetInstanceID() == componentData.TargetInstanceID);
                    SerializedObject obj = new SerializedObject(machine);
                    SerializedProperty targetProperty = obj.FindProperty(nameof(EasyStateMachine<DataTypeBase>.GenericEventHandler));
                    targetProperty.objectReferenceValue = component;
                    obj.ApplyModifiedProperties();
                }
                EasyStateSettings.Instance.ComponentAddedData = new ComponentAdded();

            }

        }
    }
}
