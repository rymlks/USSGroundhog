using EasyState.Data;
using EasyState.DataModels;
using EasyState.Models;
using EasyState.Runtime.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace EasyState.Editor.Insepctors
{
    public class SerializedEasyStateMachine
    {
        public SerializedProperty OnInitialize;
        public SerializedProperty OnPreUpdate;
        public SerializedProperty OnStateEntered;
        public SerializedProperty OnStateExited;
        public SerializedProperty OnStateUpdated;
        public SerializedProperty OnExitDelayStarted;
        public SerializedProperty OnPostUpdate;
        public SerializedProperty OnStopped;
        public SerializedProperty Behavior;
        public SerializedProperty Data;
        public SerializedProperty StartOn;
        public SerializedProperty UpdateRate;
        public SerializedProperty LogWarnings;
        public SerializedProperty UseIndividualEventHandlers;
        public SerializedProperty UseInstanceData;
        public SerializedProperty GenericEventHandler;
        public SerializedProperty RefreshCycle;

        public SerializedEasyStateMachine(SerializedObject serializedObject)
        {
            OnInitialize = serializedObject.FindProperty(nameof(EasyStateMachine.OnInitialize));
            OnPreUpdate = serializedObject.FindProperty(nameof(EasyStateMachine.OnPreUpdate));
            OnStateEntered = serializedObject.FindProperty(nameof(EasyStateMachine.OnStateEntered));
            OnStateExited = serializedObject.FindProperty(nameof(EasyStateMachine.OnStateExited));
            OnStateUpdated = serializedObject.FindProperty(nameof(EasyStateMachine.OnStateUpdated));
            OnExitDelayStarted = serializedObject.FindProperty(nameof(EasyStateMachine.OnExitDelayStarted));
            OnPostUpdate = serializedObject.FindProperty(nameof(EasyStateMachine.OnPostUpdate));
            OnStopped = serializedObject.FindProperty(nameof(EasyStateMachine.OnStopped));
            StartOn = serializedObject.FindProperty(nameof(EasyStateMachine.StartOn));
            UpdateRate = serializedObject.FindProperty(nameof(EasyStateMachine.UpdateRate));
            LogWarnings = serializedObject.FindProperty(nameof(EasyStateMachine.LogWarnings));
            UseIndividualEventHandlers = serializedObject.FindProperty(nameof(EasyStateMachine.UseIndividualEventHandlers));
            UseInstanceData = serializedObject.FindProperty(nameof(EasyStateMachine.UseInstanceData));
            Behavior = serializedObject.FindProperty(nameof(EasyStateMachine.Behavior));
            Data = serializedObject.FindProperty(nameof(EasyStateMachine<DataTypeBase>.Data));
            GenericEventHandler = serializedObject.FindProperty(nameof(EasyStateMachine<DataTypeBase>.GenericEventHandler));
            RefreshCycle = serializedObject.FindProperty(nameof(RefreshCycle));
        }
    }
    [CustomEditor(typeof(EasyStateMachine), true)]
    public class EasyStateMachineInspector : UnityEditor.Editor
    {
        private EasyStateMachine _stateMachine;
        private DataTypeCollection _dataTypeCollection;
        private List<BehaviorData> _behaviorData;
        private SerializedEasyStateMachine _serializedEasyStateMachine;
        private string[] _behaviorOptions = new string[] { };
        private int _selectedBehavior;
        private bool _eventsFoldout;
        private bool _settingsFoldout;
        private GUIStyle _boxStyle;
        private string _currentState;
        private string[] _pastStates = new string[3];
        private string _stateHistory;
        private bool _listeningForStateChanged;
        private void OnEnable()
        {

            _dataTypeCollection = new DataTypeDatabase().Load();
            if(target == null)
            {
                return;
            }

            _stateMachine = (EasyStateMachine)target;
            EasyStateFileEvents.FileChanged += LoadBehaviorData;
            _serializedEasyStateMachine = new SerializedEasyStateMachine(serializedObject);
            EditorApplication.playModeStateChanged += OnPlayModeChanged;
            LoadBehaviorData();
            if (Application.isPlaying && !_listeningForStateChanged)
            {
                ListenForStateChanges();
            }
        }

        private void OnPlayModeChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredPlayMode && !_listeningForStateChanged)
            {
                ListenForStateChanges();
            }


        }
        private void ListenForStateChanges()
        {
            var stateMachine = _stateMachine.StateMachine;
            if (stateMachine != null)
            {
                stateMachine.OnStateEntered += StateMachine_OnStateEntered;
            }
            _listeningForStateChanged = true;
        }
        private void StateMachine_OnStateEntered(State stateEntered)
        {
            if (_currentState == null)
            {
                _currentState = stateEntered.Name;
            }
            else
            {
                UpdatePreviousStates(stateEntered.Name);
            }
        }
        private void UpdatePreviousStates(string newState)
        {
            _pastStates[0] = _pastStates[1];
            _pastStates[1] = _pastStates[2];
            _pastStates[2] = _currentState;
            _currentState = newState;
            _stateHistory = string.Empty;
            for (int i = 0; i < 3; i++)
            {
                if (_pastStates[i] != null)
                {
                    _stateHistory += $" {_pastStates[i]}->";
                }
            }
            _stateHistory += $" Current State :{_currentState}";
        }


        private void LoadBehaviorData(string fileChanged = null)
        {
            if (target == null)
            {
                return;
            }
            if (fileChanged == nameof(BehaviorCollectionData) || fileChanged == null)
            {
                var allDataTypes = new DataTypeDatabase().Load().DataTypes;
                var allBehaviors = new BehaviorCollectionDatabase().Load().Behaviors;
                _behaviorData = new List<BehaviorData>();
                var currentDataTypeID = _stateMachine.DataTypeID;
                while (!string.IsNullOrEmpty(currentDataTypeID))
                {
                    var currentDataType = allDataTypes.First(x => x.Id == currentDataTypeID);
                    _behaviorData.AddRange(allBehaviors.Where(x => x.DataTypeId == currentDataTypeID));
                    currentDataTypeID = currentDataType.ParentDataTypeID;
                }

                _behaviorOptions = _behaviorData.Select(x => x.BehaviorName).ToArray();
                if (_stateMachine?.Behavior?.BehaviorName != null && !string.IsNullOrEmpty(_stateMachine.Behavior?.DesignId))
                {
                    _stateMachine.Behavior = _behaviorData.FirstOrDefault(x => x.DesignId == _stateMachine.Behavior.DesignId);
                    if (_stateMachine?.Behavior?.BehaviorName != null)
                    {
                        _selectedBehavior = _behaviorOptions.ToList().IndexOf(_stateMachine.Behavior.BehaviorName);
                    }
                }
            }
        }


        public override void OnInspectorGUI()
        {
            if (string.IsNullOrEmpty(_stateMachine.StateMachineComponentID))
            {
                _stateMachine.StateMachineComponentID = Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }

            if (!_behaviorOptions.Any())
            {
                EditorGUILayout.HelpBox("This data type doesn't have any behaviors. Create behaviors in the designer before using this component.", MessageType.Warning, true);
                _stateMachine.Behavior = null;
                return;
            }
            _boxStyle = new GUIStyle("HelpBox");
            _boxStyle.padding = new RectOffset(10, 10, 10, 10);

            EditorGUILayout.LabelField("Easy State Machine", new GUIStyle("WhiteLargeLabel"));
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical(_boxStyle);
            GUI.enabled = !Application.isPlaying;
            _selectedBehavior = EditorGUILayout.Popup("Behavior", _selectedBehavior, _behaviorOptions);
            GUI.enabled = true;
            _stateMachine.Behavior = GetSelectedBehavior();
            try
            {
                EditorGUILayout.PropertyField(_serializedEasyStateMachine.Data);
            }
            catch (System.InvalidOperationException)
            {
                //catching InvalidOperationException: Stack empty. that shows up randomly 
            }
            EditorGUILayout.EndVertical();
            EditorGUI.indentLevel++;
            if (!Application.isPlaying)
            {
                DrawEvents();
                DrawSettings();
            }
            else
            {
                DrawMachineControls();
            }

            EditorGUI.indentLevel--;
            if (GUI.changed)
            {
                serializedObject.ApplyModifiedProperties();
            }
            if (Application.isPlaying)
            {
                GUI.changed = true;
            }

        }

        private void DrawMachineControls()
        {
            EditorGUILayout.BeginHorizontal();
            if (_stateMachine.UpdateRate != StateMachineUpdateRate.None)
            {
                if (_stateMachine.IsRunning)
                {
                    if (GUILayout.Button(new GUIContent("Stop", "Stop state machine from updating"), GUILayout.ExpandWidth(false)))
                    {
                        _stateMachine.StopMachine();
                    }
                }
                else
                {
                    if (GUILayout.Button(new GUIContent("Start", "Start state machine updating"), GUILayout.ExpandWidth(false)))
                    {
                        _stateMachine.StartMachine();
                    }
                }
            }
            if (GUILayout.Button(new GUIContent("Update", "Update the state machine once"), GUILayout.ExpandWidth(false)))
            {
                _stateMachine.UpdateStateMachine();
            }
            if (_stateMachine.UpdateRate != StateMachineUpdateRate.None)
            {
                if (GUILayout.Button(new GUIContent("Recycle", "Stop and restart the state machine updating"), GUILayout.ExpandWidth(false)))
                {
                    _stateMachine.RecycleMachine();
                }
            }
            EditorGUILayout.EndHorizontal();
            if (_stateHistory != null)
            {
                EditorGUILayout.LabelField(_stateHistory);
            }

        }

        private void DrawEvents()
        {
            if (_stateMachine.UseIndividualEventHandlers)
            {
                _stateMachine.ClearSingleEventHandler();
                _eventsFoldout = EditorGUILayout.Foldout(_eventsFoldout, new GUIContent("Events"));
                if (_eventsFoldout)
                {
                    EditorGUILayout.BeginVertical(_boxStyle);
                    EditorGUILayout.PropertyField(_serializedEasyStateMachine.OnInitialize);
                    EditorGUILayout.PropertyField(_serializedEasyStateMachine.OnPreUpdate);
                    EditorGUILayout.PropertyField(_serializedEasyStateMachine.OnStateEntered);
                    EditorGUILayout.PropertyField(_serializedEasyStateMachine.OnStateUpdated);
                    EditorGUILayout.PropertyField(_serializedEasyStateMachine.OnStateExited);
                    EditorGUILayout.PropertyField(_serializedEasyStateMachine.OnExitDelayStarted);
                    EditorGUILayout.PropertyField(_serializedEasyStateMachine.OnPostUpdate);
                    EditorGUILayout.PropertyField(_serializedEasyStateMachine.OnStopped);
                    EditorGUILayout.EndVertical();

                }
            }
            else
            {
                ClearSingleEventHandlers();
                var rect = EditorGUILayout.BeginVertical();
                try
                {
                    EditorGUI.PropertyField(rect, _serializedEasyStateMachine.GenericEventHandler);
                }
                catch (InvalidOperationException)
                {
                    //catching InvalidOperationException: Stack empty. that shows up randomly 
                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space(EditorGUIUtility.singleLineHeight, true);
            }
        }
        private void DrawSettings()
        {
            _settingsFoldout = EditorGUILayout.Foldout(_settingsFoldout, new GUIContent("Settings"));
            if (_settingsFoldout)
            {
                EditorGUILayout.BeginVertical(_boxStyle);
                EditorGUILayout.PropertyField(_serializedEasyStateMachine.StartOn);
                if (RequiresWarningMessageForRate())
                {
                    EditorGUILayout.HelpBox("Use this setting with care, as many Unity APIs are not supported in background threads", MessageType.Warning);
                }
                else if (_stateMachine.UpdateRate == StateMachineUpdateRate.None)
                {
                    EditorGUILayout.HelpBox("State exit delays will be ignored with 'Update Rate' set to none", MessageType.Warning);
                }
                EditorGUILayout.PropertyField(_serializedEasyStateMachine.UpdateRate);
                if (_stateMachine.UpdateRate.RequiresCustomRate())
                {
                    EditorGUILayout.PropertyField(_serializedEasyStateMachine.RefreshCycle);
                }
                EditorGUILayout.PropertyField(_serializedEasyStateMachine.LogWarnings);
                EditorGUILayout.PropertyField(_serializedEasyStateMachine.UseInstanceData);
                EditorGUILayout.PropertyField(_serializedEasyStateMachine.UseIndividualEventHandlers, new GUIContent("Individual Events"));
                EditorGUILayout.SelectableLabel("Component Id : " + _stateMachine.StateMachineComponentID);
                EditorGUILayout.EndVertical();
            }
        }

        public bool RequiresWarningMessageForRate()
        {
            return _stateMachine.UpdateRate == StateMachineUpdateRate.BackgroundWithEventSync ||
                 _stateMachine.UpdateRate == StateMachineUpdateRate.BackgroundThread;
        }
        private void ClearSingleEventHandlers()
        {
            _stateMachine.OnInitialize = null;
            _stateMachine.OnStateEntered = null;
            _stateMachine.OnStateUpdated = null;
            _stateMachine.OnStateExited = null;
            _stateMachine.OnExitDelayStarted = null;
            _stateMachine.OnStopped = null;
        }
        private BehaviorData GetSelectedBehavior() => _behaviorData[_selectedBehavior];
        private void OnDisable()
        {
            EasyStateFileEvents.FileChanged -= LoadBehaviorData;
            EditorApplication.playModeStateChanged -= OnPlayModeChanged;
            if (_stateMachine?.StateMachine != null)
            {
                _stateMachine.StateMachine.OnStateEntered -= StateMachine_OnStateEntered;
            }

        }
    }

}