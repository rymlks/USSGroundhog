#if UNITY_EDITOR
using EasyState.Settings;
using UnityEditor.Callbacks;
using UnityEngine;
namespace EasyState.Data.FileSync
{
    public class FileWatcher
    {
        public static event System.Action FileChanged;
        [DidReloadScripts]
        private static void OnScriptReload()
        {
            var dataTypeDB = new DataTypeDatabase().Load();
            var designs = new DesignCollectionFullDatabase().Load();
            var designWindowState = new DesignWindowDatabase().Load();
            var dataTypeChanges = new DataTypeChanges(dataTypeDB.DataTypes);

            var settings = EasyStateSettings.Instance;
            if (settings.LogFileChanges && (dataTypeChanges.ChangesDetected))
            {
                Debug.Log("Data change detected, to turn off change detection logging, set 'LogFileChanges' to false in EasyState Settings");
            }
            if (dataTypeChanges.ChangesDetected)
            {
                ChangeHandler.HandleChanges(dataTypeChanges, dataTypeDB, designs, designWindowState);
            }
            var functionChanges = new FunctionChanges(dataTypeDB.DataTypes);
            if (settings.LogFileChanges && (!dataTypeChanges.ChangesDetected) && functionChanges.ChangesDetected)
            {
                Debug.Log("Data change detected, to turn off change detection logging, set 'LogFileChanges' to false in EasyState Settings");
            }
            if (functionChanges.ChangesDetected)
            {
                ChangeHandler.HandleChanges(functionChanges, dataTypeDB, designs, designWindowState);
            }
            if (dataTypeChanges.ChangesDetected || functionChanges.ChangesDetected)
            {
                FileChanged?.Invoke();
            }
        }
    }
} 
#endif