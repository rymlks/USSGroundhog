using UnityEditor;
using UnityEngine;
using System;
using EasyState.Runtime;
using EasyState.Core.Utility;
using System.IO;

namespace EasyState.Editor.Debugger
{
    public class DebuggerWindow : EditorWindow, IHasCustomMenu
    {
        public static DebuggerWindow Instance { get; private set; }
        public static bool IsOpen
        {
            get { return Instance != null; }
        }
        public string stateMachineComponentID;
        [NonSerialized]
        private EasyStateMachine _stateMachine;
        public event Action<EasyStateMachine> EditorEnteredPlayMode;
        public event Action EditorExitedPlayMode;
        private void OnEnable()
        {
            CreateWindow();
        }

        private void OnPlayModeChanged(PlayModeStateChange obj)
        {
            if (obj == PlayModeStateChange.EnteredPlayMode && _bootstrapper != null)
            {
           
                if (!string.IsNullOrEmpty(stateMachineComponentID))
                {

                    _stateMachine = EasyStateMachineComponentCache.GetComponent(stateMachineComponentID);
                    //this will not be null at runtime but will be null in the editor so clear the value
                    if (_stateMachine != null)
                    {
                        _bootstrapper.SetStateMachine(_stateMachine);
                    }
                    else
                    {
                        stateMachineComponentID = null;
                    }
                }

                EditorEnteredPlayMode?.Invoke(_stateMachine);
            }
            else if (obj == PlayModeStateChange.ExitingPlayMode && _bootstrapper != null)
            {
                EditorExitedPlayMode?.Invoke();
            }
        }

        [MenuItem("Window/Easy State/Debugger")]
        public static void OpenWindow()
        {
            DebuggerWindow wnd = GetWindow<DebuggerWindow>();
            var icon = AssetDatabase.LoadAssetAtPath<Texture2D>(Path.Combine(FilePaths.EditorImageFolder, "Debug.png"));
            wnd.titleContent = new GUIContent("Debugger", icon);

        }
        private DebuggerWindowBootstrapper _bootstrapper;
        public void CreateWindow()
        {
            if(Instance == null)
            {
                Instance = this;
                EditorApplication.playModeStateChanged += OnPlayModeChanged;
            }
            if (_bootstrapper != null)
            {
                _bootstrapper.Dispose();
            }
            _bootstrapper = new DebuggerWindowBootstrapper(rootVisualElement);
            if (Application.isPlaying)
            {
                EditorEnteredPlayMode?.Invoke(_stateMachine);
            } 
        }
        public void OnChangeDesign()
        {
            _stateMachine = null;
            stateMachineComponentID = null;
            CreateWindow();
        }
        public void AddItemsToMenu(GenericMenu menu)
        {
            menu.AddItem(new GUIContent("Rebuild Debugger"), false, CreateWindow);
        }
        public void OnStateMachineConfirmed(EasyStateMachine stateMachine)
        {
            var serializedObj = new SerializedObject(this);
            stateMachineComponentID = stateMachine.StateMachineComponentID;
            serializedObj.ApplyModifiedProperties();
            _bootstrapper.SetStateMachine(stateMachine);

        }
        private void OnDisable()
        {
            if (_bootstrapper != null)
            {
                _bootstrapper.Dispose();
                _bootstrapper = null;
                EditorApplication.playModeStateChanged -= OnPlayModeChanged;
            }
        }
    }
}