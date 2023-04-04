using System;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace EasyState.Editor.Utility
{
    public static class ScriptGenerator
    {
        private const string _classNameTerm = "$class-name";
        private const string _scriptIDTerm = "$script-id";
        private const string _dataTypeTerm = "$data-type";
        private const string _genericPlaceHolder = "TDataType";
        private const string _additionalUsingNamespace = "$using-namespace";
        private static readonly string _templateFolderPath = Path.Combine(Settings.EasyStateSettings.Instance.EasyStateFolder, "Editor", "Templates");


        //[MenuItem("Assets/EasyState/New Action")]
        //public static void CreateAction() => CreateAction(null);
        public static void CreateAction(string dataType)
        {
            string filePath = GetFilePathFromUser("Create New Action", "Action1");
            if (!string.IsNullOrEmpty(filePath))
            {
                var fileName = Path.GetFileNameWithoutExtension(filePath);
                StringBuilder sb = new StringBuilder();
                sb.Append(ReadFile("ActionTemplate"));
                sb.Replace(_classNameTerm, fileName);
                sb.Replace(_scriptIDTerm, Guid.NewGuid().ToString());
                if (dataType != null)
                {
                    sb.Replace(_genericPlaceHolder, dataType);
                }
                File.WriteAllText(filePath, sb.ToString());
                EasyState.Settings.EasyStateSettings.Instance.ComponentCreationDetails.NewActionFilePath = filePath;
                AssetDatabase.Refresh();
            }
        }
        //[MenuItem("Assets/EasyState/New Condition")]
        //public static void CreateCondition() => CreateCondition(null);
        public static void CreateCondition(string dataType)
        {
            string filePath = GetFilePathFromUser("Create New Condition", "Condition1");
            if (!string.IsNullOrEmpty(filePath))
            {
                var fileName = Path.GetFileNameWithoutExtension(filePath);
                StringBuilder sb = new StringBuilder();
                sb.Append(ReadFile("ConditionTemplate"));
                sb.Replace(_classNameTerm, fileName);
                sb.Replace(_scriptIDTerm, Guid.NewGuid().ToString());
                if (dataType != null)
                {
                    sb.Replace(_genericPlaceHolder, dataType);
                }
                File.WriteAllText(filePath, sb.ToString());
                EasyState.Settings.EasyStateSettings.Instance.ComponentCreationDetails.NewConditionFilePath = filePath;
                AssetDatabase.Refresh();
            }
        }
        //[MenuItem("Assets/EasyState/New Evaluator")]
        //public static void CreateEvaluator() => CreateEvaluator(null);
        public static void CreateEvaluator(string dataType)
        {
            string filePath = GetFilePathFromUser("Create New Evaluator", "Evaluator1");
            if (!string.IsNullOrEmpty(filePath))
            {
                var fileName = Path.GetFileNameWithoutExtension(filePath);
                StringBuilder sb = new StringBuilder();
                sb.Append(ReadFile("EvaluatorTemplate"));
                sb.Replace(_classNameTerm, fileName);
                sb.Replace(_scriptIDTerm, Guid.NewGuid().ToString());
                if (dataType != null)
                {
                    sb.Replace(_genericPlaceHolder, dataType);
                }
                File.WriteAllText(filePath, sb.ToString());
                EasyState.Settings.EasyStateSettings.Instance.ComponentCreationDetails.NewEvaluatorFilePath = filePath;
                AssetDatabase.Refresh();
            }
        }
        [MenuItem("Assets/EasyState/New Data Type")]
        public static void CreateDataType()
        {
            string filePath = GetFilePathFromUser("Create New Data Type", "DataType1");
            if (!string.IsNullOrEmpty(filePath))
            {
                var creationDetails = EasyState.Settings.EasyStateSettings.Instance.ComponentCreationDetails;
                //save data type file
                var fileName = Path.GetFileNameWithoutExtension(filePath);
                StringBuilder sb = new StringBuilder();
                sb.Append(ReadFile("DataTypeTemplate"));
                sb.Replace(_classNameTerm, fileName);
                sb.Replace(_scriptIDTerm, Guid.NewGuid().ToString());
                File.WriteAllText(filePath, sb.ToString());
                //save state machine file
                var sb2 = new StringBuilder();
                var stateMachineFileName = $"{fileName}StateMachine.cs";
                sb2.Append(ReadFile("StateMachineTemplate"));
                sb2.Replace(_dataTypeTerm, fileName);
                var stateMachineFilePath = filePath.Replace(Path.GetFileName(filePath), stateMachineFileName);
                File.WriteAllText(stateMachineFilePath, sb2.ToString());
                creationDetails.NewStateMachineFilePath = stateMachineFilePath;
                creationDetails.NewDataTypeFilePath = filePath;


                AssetDatabase.Refresh();
            }
        }
        //[MenuItem("Assets/EasyState/New Parameterized Action")]
        //public static void CreateParameterizedAction() => CreateParameterizedAction(null);
        public static void CreateParameterizedAction(string dataType)
        {
            string filePath = GetFilePathFromUser("Create New Parameterized Action", "Action1");
            if (!string.IsNullOrEmpty(filePath))
            {
                var fileName = Path.GetFileNameWithoutExtension(filePath);
                StringBuilder sb = new StringBuilder();
                sb.Append(ReadFile("ParameterizedActionTemplate"));
                sb.Replace(_classNameTerm, fileName);
                sb.Replace(_scriptIDTerm, Guid.NewGuid().ToString());
                if (dataType != null)
                {
                    sb.Replace(_genericPlaceHolder, dataType);
                }
                File.WriteAllText(filePath, sb.ToString());
                EasyState.Settings.EasyStateSettings.Instance.ComponentCreationDetails.NewActionFilePath = filePath;
                AssetDatabase.Refresh();
            }
        }
        public static string GetEventHandlerTemplate(string className, string dataType, string usingNamespace)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(ReadFile("EventHandlerTemplate"));
            sb.Replace(_classNameTerm, className);
            sb.Replace(_dataTypeTerm, dataType);
            sb.Replace(_additionalUsingNamespace, usingNamespace);
            return sb.ToString();
        }
        private static string GetCurrentEditorFilePath()
        {
            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (File.Exists(path))
                path = Path.GetDirectoryName(path);
            if (string.IsNullOrEmpty(path)) path = "Assets/";
            return path;
        }

        private static string GetFilePathFromUser(string popupTitle, string defaultFileName)
        {
            string currentDir = GetCurrentEditorFilePath();
            string filePath = EditorUtility.SaveFilePanel(popupTitle, currentDir, defaultFileName, "cs");
            return filePath;
        }

        private static string ReadFile(string fileName)
        {
            return File.ReadAllText(Path.Combine(_templateFolderPath, fileName + ".txt"));
        }
        private const string _defaultIconContent = "{instanceID: 0}";
        [DidReloadScripts]
        private static void OnScriptsReload()
        {
            var settings = Settings.EasyStateSettings.Instance;
            var creationData = settings.ComponentCreationDetails;
            if (settings.UseCustomIcons)
            {
                //set the state machine icon
                if (!string.IsNullOrEmpty(creationData.NewStateMachineFilePath))
                {
                    SetIconForScript(creationData.NewStateMachineFilePath, settings.StateMachineIcon);
                    creationData.NewStateMachineFilePath = null;
                }
                //set the data type icon
                if (!string.IsNullOrEmpty(creationData.NewDataTypeFilePath))
                {

                    SetIconForScript(creationData.NewDataTypeFilePath, settings.DataTypeIcon);
                    //Create an instance of the data type's scriptable object
                    string fileName = Path.GetFileNameWithoutExtension(creationData.NewDataTypeFilePath);
                    var dataTypeAsset = UnityEngine.ScriptableObject.CreateInstance(fileName);
                    var assetPath = RelativePath(creationData.NewDataTypeFilePath.Replace(".cs", ".asset"));
                    AssetDatabase.CreateAsset(dataTypeAsset, assetPath);
                    creationData.NewDataTypeFilePath = null;
                }
                //set the action type icon
                if (!string.IsNullOrEmpty(creationData.NewActionFilePath))
                {
                    SetIconForScript(creationData.NewActionFilePath, settings.ActionIcon);
                    creationData.NewActionFilePath = null;
                }
                //set the condition type icon
                if (!string.IsNullOrEmpty(creationData.NewConditionFilePath))
                {
                    SetIconForScript(creationData.NewConditionFilePath, settings.ConditionIcon);
                    creationData.NewConditionFilePath = null;
                }
                //set the evaluator type icon
                if (!string.IsNullOrEmpty(creationData.NewEvaluatorFilePath))
                {
                    SetIconForScript(creationData.NewEvaluatorFilePath, settings.EvaluatorIcon);
                    creationData.NewEvaluatorFilePath = null;
                }
                //set the event handler type icon
                if (!string.IsNullOrEmpty(creationData.NewEventHandlerFilePath))
                {
                    SetIconForScript(creationData.NewEventHandlerFilePath, settings.EventHandlerIcon);
                    creationData.NewEventHandlerFilePath = null;
                }


            }
            else
            {
                if (!string.IsNullOrEmpty(creationData.NewDataTypeFilePath))
                {
                    //Create an instance of the data type's scriptable object
                    string fileName = Path.GetFileNameWithoutExtension(creationData.NewDataTypeFilePath);
                    var dataTypeAsset = UnityEngine.ScriptableObject.CreateInstance(fileName);
                    var assetPath = RelativePath(creationData.NewDataTypeFilePath.Replace(".cs", ".asset"));
                    AssetDatabase.CreateAsset(dataTypeAsset, assetPath);
                    creationData.NewDataTypeFilePath = null;
                }
                creationData.NewStateMachineFilePath = null;
                creationData.NewDataTypeFilePath = null;
                creationData.NewActionFilePath = null;
                creationData.NewConditionFilePath = null;
                creationData.NewEvaluatorFilePath = null;
                creationData.NewEventHandlerFilePath = null;
            }
        }
        private static string RelativePath(string path)
        {
            if (path.StartsWith(Application.dataPath))
            {
                path = "Assets" + path.Substring(Application.dataPath.Length);
            }
            return path;
        }
        private static MethodInfo _setIconMethod = typeof(EditorGUIUtility).GetMethod("SetIconForObject", BindingFlags.Static | BindingFlags.NonPublic);
        private static MethodInfo _copyMonoScriptMethod = typeof(MonoImporter).GetMethod("CopyMonoScriptIconToImporters", BindingFlags.Static | BindingFlags.NonPublic);
        private static void SetIconForScript(string absoluteFilePath, Texture2D icon)
        {
#if UNITY_2021_2_OR_NEWER

            var monoImporter = AssetImporter.GetAtPath(RelativePath(absoluteFilePath)) as MonoImporter;            
            monoImporter.SetIcon(icon);
            monoImporter.SaveAndReimport();
#else
            var script = AssetDatabase.LoadAssetAtPath<MonoScript>(RelativePath(absoluteFilePath));         
            _setIconMethod.Invoke(null, new object[] { script, icon });
            _copyMonoScriptMethod.Invoke(null, new object[] { script });

#endif
        }
    }
}