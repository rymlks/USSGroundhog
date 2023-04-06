using EasyState.Core;
using EasyState.Data;
using EasyState.DataModels;
using EasyState.Models;
using EasyState.Runtime;
using EasyState.Runtime.Utility;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace EasyState
{
    [System.Serializable]
    public class StateEvent : UnityEvent<State> { }
    [System.Serializable]
    public class StateDelayEvent : UnityEvent<float> { }
    [System.Serializable]
    public class StateMachineEvent : UnityEvent<EasyStateMachine> { }
    public enum UnityTimelineEvents { OnEnable, OnAwake, Start, FirstUpdate, Never }
    public enum StateMachineUpdateRate { None, Update, LateUpdate, FixedUpdate, CustomRefreshRate, BackgroundWithEventSync, BackgroundThread }
    [System.Serializable]
    public abstract class EasyStateMachine : MonoBehaviour, IEventController
    {
        public string StateMachineComponentID;
        public StateMachineEvent OnInitialize;
        public UnityEvent OnPreUpdate;
        public StateEvent OnStateEntered;
        public StateEvent OnStateExited;
        public StateEvent OnStateUpdated;
        public StateDelayEvent OnExitDelayStarted;
        public UnityEvent OnPostUpdate;
        public StateMachineEvent OnStopped;
        public UnityTimelineEvents StartOn;
        public StateMachineUpdateRate UpdateRate;
        public BehaviorData Behavior;
        public bool LogWarnings = true;
        public bool UseIndividualEventHandlers = true;
        public bool UseInstanceData = true;
        public bool IsRunning;
        public float RefreshCycle = .01f;
        public IStateMachine StateMachine { get; protected set; }
        protected bool _isInitialized;
        protected IEventController _eventController;
        private EasyRefresher.Machine _machine;
        private EasyRefresher.CustomRateMachine _customRateMachine;
        private bool _firstUpdate;
        private bool _onInitializedCalled;
        public abstract string DataTypeID { get; }
        protected void LogWarning(string warning)
        {
            if (LogWarnings)
            {
                Debug.LogWarning(warning);
            }
        }
        protected virtual void Awake()
        {
            EasyStateFileEvents.FileChanged += OnBehaviorsUpdated;
            EasyStateMachineComponentCache.AddComponent(this);
            InitializeStateMachine();

            if (!_isInitialized)
            {
                return;
            }

            if (StartOn == UnityTimelineEvents.OnAwake)
            {
                StartMachine();
            }
        }
        private void InitializeStateMachine(string startingStateID = null)
        {
            if (!PreLoadValidateMachine())
            {
                return;
            }
            LoadStateMachine(startingStateID);
            if (UseIndividualEventHandlers && StateMachine != null)
            {
                StateMachine.OnStateEntered += OnStateEntered.Invoke;
                StateMachine.OnStateExited += OnStateExited.Invoke;
                StateMachine.OnStateUpdated += OnStateUpdated.Invoke;
                StateMachine.OnExitDelayStarted += OnExitDelayStarted.Invoke;
                _eventController = this;
            }
            else
            {
                _eventController = GetEventController();
            }
            PostLoadValidateMachine();
        }
        private void OnBehaviorsUpdated(string fileName)
        {
            if (fileName == nameof(BehaviorCollectionData))
            {

                if (_isInitialized)
                {
                    var currentStateId = StateMachine.CurrentState.Id;
                    bool wasRunning = IsRunning;
                    StopMachine();
                    Database.ClearCache();
                    InitializeStateMachine(currentStateId);

                    if (wasRunning)
                    {
                        StartMachine();
                    }
                }
            }
        }
        protected virtual void OnEnable()
        {
            if (StartOn == UnityTimelineEvents.OnEnable)
            {
                StartMachine();
            }
        }
        protected virtual void Start()
        {
            if (StartOn == UnityTimelineEvents.Start)
            {
                StartMachine();
            }
        }
        protected virtual void Update()
        {
            if (!_firstUpdate)
            {
                _firstUpdate = true;
                if (StartOn == UnityTimelineEvents.FirstUpdate)
                {
                    StartMachine();
                }
            }
        }
        public void StartMachine()
        {
            if (_isInitialized)
            {
                AddRefresher();
                _onInitializedCalled = true;
            }
        }
        public void RecycleMachine()
        {
            if (_isInitialized)
            {
                StopMachine();
                StateMachine.Reset();
                StartMachine();
            }
        }
        public void StopMachine()
        {
            if (_isInitialized)
            {
                RemoveRefresher();
                _eventController.OnStopped(this);
            }
        }
        protected virtual void OnDisable()
        {
            if (gameObject.scene.isLoaded)
            {
                StopMachine();
                if (_isInitialized && UseIndividualEventHandlers)
                {
                    StateMachine.OnStateEntered -= OnStateEntered.Invoke;
                    StateMachine.OnStateExited -= OnStateExited.Invoke;
                    StateMachine.OnStateUpdated -= OnStateUpdated.Invoke;
                    StateMachine.OnExitDelayStarted -= OnExitDelayStarted.Invoke;
                }
            }
            EasyStateFileEvents.FileChanged -= OnBehaviorsUpdated;
        }

        private void AddRefresher()
        {
            if (!_onInitializedCalled)
            {
                _eventController.OnInitialize(this, GetDataInstance());
            }

            switch (UpdateRate)
            {
                case StateMachineUpdateRate.Update:
                    _machine = EasyRefresher.Instance.AddUpdateRefresher(StateMachine, _eventController);
                    break;

                case StateMachineUpdateRate.FixedUpdate:
                    _machine = EasyRefresher.Instance.AddFixedUpdateRefresher(StateMachine, _eventController);

                    break;

                case StateMachineUpdateRate.LateUpdate:
                    _machine = EasyRefresher.Instance.AddLateUpdateRefresher(StateMachine, _eventController);
                    break;

                case StateMachineUpdateRate.CustomRefreshRate:
                    _customRateMachine = EasyRefresher.Instance.AddCustomRateRefresher(StateMachine, _eventController, RefreshCycle);
                    break;

                case StateMachineUpdateRate.BackgroundWithEventSync:
                    _customRateMachine = EasyRefresher.Instance.AddBackgroundWithEventSync(StateMachine, _eventController, RefreshCycle);
                    break;

                case StateMachineUpdateRate.BackgroundThread:
                    _customRateMachine = EasyRefresher.Instance.AddBackgroundRefresher(StateMachine, _eventController, RefreshCycle);
                    break;

                default:
                    break;
            }
            IsRunning = true;
        }
        public void UpdateStateMachine()
        {
            if (_isInitialized)
            {
                _eventController.OnPreUpdate(GetDataInstance());
                StateMachine.Update();
                _eventController.OnPostUpdate(GetDataInstance());
            }

        }
        private void RemoveRefresher()
        {
            switch (UpdateRate)
            {
                case StateMachineUpdateRate.Update:
                    EasyRefresher.Instance.StopUpdateRefresher(_machine);
                    break;

                case StateMachineUpdateRate.FixedUpdate:
                    EasyRefresher.Instance.StopFixedUpdateRefresher(_machine);
                    break;

                case StateMachineUpdateRate.LateUpdate:
                    EasyRefresher.Instance.StopLateUpdateRefresher(_machine);
                    break;

                case StateMachineUpdateRate.CustomRefreshRate:
                case StateMachineUpdateRate.BackgroundWithEventSync:
                case StateMachineUpdateRate.BackgroundThread:
                    EasyRefresher.Instance.StopCustomRateMachine(_customRateMachine);
                    break;
                default:
                    break;
            }
            IsRunning = false;

        }
        public string GetCurrentStateName() => StateMachine?.CurrentState?.Name;
        protected abstract void LoadStateMachine(string startingStateID = null);
        public abstract void ClearSingleEventHandler();
        public abstract IEventController GetEventController();
        void IEventController.OnInitialize(EasyStateMachine stateMachine, DataTypeBase data) => OnInitialize?.Invoke(this);
        void IEventController.OnPostUpdate(DataTypeBase data) => OnPostUpdate?.Invoke();
        void IEventController.OnPreUpdate(DataTypeBase data) => OnPreUpdate?.Invoke();
        void IEventController.OnStopped(EasyStateMachine stateMachine) => OnStopped?.Invoke(this);
        public abstract bool PreLoadValidateMachine();
        public abstract bool PostLoadValidateMachine();
        public abstract DataTypeBase GetDataInstance();
        private void OnValidate()
        {
            var current = Event.current;

            if (current?.type == EventType.ExecuteCommand && current.commandName == "Duplicate")
            { 
                StateMachineComponentID = Guid.NewGuid().ToString(); 
            }
        }

    }
    public abstract class EasyStateMachine<T> : EasyStateMachine where T : DataTypeBase
    {
        [DataType]
        public T Data;
        public Core.EventHandler<T> EventHandler => GenericEventHandler as Core.EventHandler<T>;
        [EasyStateEventHandler]
        public MonoBehaviour GenericEventHandler;
        protected override void LoadStateMachine(string startingStateID = null)
        {
            if (UseInstanceData)
            {
                Data = Instantiate(Data);
            }
            Data.FSM_GameObject = this.gameObject;
            Data.FSM_Transform = this.transform;
            try
            {
                StateMachine = new StateMachine<T>(this, Data, Behavior.DesignId, startingStateID);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                Debug.Log("Ran into error while attempting to load state machine. Make sure the behavior hasn't been deleted.");
                Debug.LogException(ex);

            }
        }
        public override void ClearSingleEventHandler() => GenericEventHandler = null;
        public override IEventController GetEventController()
        {
            if (EventHandler is null)
            {
                return NullEventController.Default;
            }
            else
            {
                return EventHandler;
            }
        }
        public override bool PreLoadValidateMachine()
        {

            if (Behavior is null)
            {
                LogWarning("State machine can not be loaded because Behavior is null.");
                return false;
            }
            if (Data is null)
            {
                LogWarning("This machine can not start up because its data field is null.");
                return false;
            }
            if (RefreshCycle == 0 && UpdateRate.RequiresCustomRate())
            {
                LogWarning("Refresh Cycle is 0, changing to 0.01f to avoid deadlocks.");
                RefreshCycle = .01f;
            }
            return true;
        }
        public override bool PostLoadValidateMachine()
        {
            if (StateMachine is null)
            {
                //If running into consistent issues catch the error on ln248 screenshot the stack trace and send an email to 
                //pigeonstudios.dev@gmail.com
                LogWarning("Failed to load state machine, make sure behavior file/data has not been deleted.");
                return false;
            }
            if (_eventController is null)
            {
                LogWarning("Failed to set event controller");
                return false;
            }
            if (EventHandler != null)
            {
                StateMachine.OnStateEntered += StateEntered;
                StateMachine.OnStateUpdated += StateUpdated;
                StateMachine.OnStateExited += StateExited;
                StateMachine.OnExitDelayStarted += ExitDelayStarted;
            }
            _isInitialized = true;
            return true;
        }
        public override DataTypeBase GetDataInstance() => Data;

        protected override void OnDisable()
        {
            base.OnDisable();
            if (EventHandler != null && StateMachine != null)
            {
                StateMachine.OnStateEntered -= StateEntered;
                StateMachine.OnStateUpdated -= StateUpdated;
                StateMachine.OnStateExited -= StateExited;
                StateMachine.OnExitDelayStarted -= ExitDelayStarted;
            }
        }
        private void StateEntered(State stateEntered) => EventHandler.OnStateEntered(stateEntered, Data);
        private void StateUpdated(State stateUpdated) => EventHandler.OnStateUpdated(stateUpdated, Data);
        private void StateExited(State stateExited) => EventHandler.OnStateExited(stateExited, Data);
        private void ExitDelayStarted(float delayAmount) => EventHandler.OnExitDelayStarted(StateMachine, delayAmount);

    }
}
