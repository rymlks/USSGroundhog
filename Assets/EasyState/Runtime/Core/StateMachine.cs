using EasyState.Data;
using EasyState.DataModels;
using EasyState.Factory;
using EasyState.Models;
using System;
using System.Linq;

namespace EasyState.Core
{
    public abstract class StateMachine
    {
        public delegate void ExitStateCallback(State stateExited);
        public delegate void EnterStateCallback(State stateEntered);
        public delegate void StateUpdatedCallback(State stateUpdated);
        public delegate void ExitDelayStartedCallback(float delayAmount);
        public delegate void ActionExecuted(string nameOfActionExecuted);
    }
    public class StateMachine<T> : StateMachine, IStateMachine where T : DataTypeBase
    {
        private enum TransitionState { Enter, Update, Exit, Resolve, Stop, Delay }
        public event EnterStateCallback OnStateEntered;
        public event StateUpdatedCallback OnStateUpdated;
        public event ExitStateCallback OnStateExited;
        public event ExitDelayStartedCallback OnExitDelayStarted;
        public event ActionExecuted OnActionExecuted;
        public event Action OnUpdateCycleCompleted;

        public State CurrentState => _currentState;
        private State<T> _currentState;
        public ModelData CurrentStateModelData => _currentState;
        public State NextState => _nextState;
        public T DataInstance { get; private set; }
        public DataTypeBase DataBaseInstance => DataInstance;

        public EasyStateMachine StateMachineComponent { get; }

        private readonly State<T> _initialState;
        private State<T> _nextState;
        private const int MAX_UPDATE_CYCLE = 500;
        private int _currentCycle = 0;
        private readonly string _designName;
        private TransitionState _transitionState;
        private bool _updating;
        public StateMachine(EasyStateMachine stateMachineComponent, T dataTypeInstance, string designID, string startingStateID = null)
        {
            DataInstance = dataTypeInstance;
            var designData = Database.GetDesignData(designID);
            _designName = designData.Name;
            var states = StateMachineFactory<T>.LoadStates(designData, this);
            if (startingStateID == null || states.All(x => x.Id != startingStateID))
            {
                _initialState = states.First(x => x.StateType == StateType.Entry && x.DesignID == designID);
            }
            else
            {
                _initialState = states.First(x => x.Id == startingStateID);
            }
            _currentState = _initialState;
            _transitionState = TransitionState.Enter;
            StateMachineComponent = stateMachineComponent;
        }

        public void Reset() => _currentState = _initialState;
        /// <summary>
        /// Advances the state machine forward one execution cycle, with a safety check stopping incorrect 
        /// state machine configurations from causing an infinite loop and causing a stack overflow error.
        /// </summary>
        public void Update()
        {
            _updating = true;
            while (_updating)
            {
                if (_currentCycle++ > MAX_UPDATE_CYCLE)
                {
                    UnityEngine.Debug.LogWarning($"State machine with data type: {typeof(T).Name}, design: {_designName} state: {CurrentState.Name} exceeded the max update limit of {MAX_UPDATE_CYCLE}, this is most likely caused by an infinite loop. Double check node cycle types to make sure " +
                        $"your state machine is not getting stuck inside a node.");
                    break;
                }
                switch (_transitionState)
                {
                    case TransitionState.Enter:
                        EnterState();
                        break;
                    case TransitionState.Update:
                        UpdateState();
                        break;
                    case TransitionState.Exit:
                        ExitState();
                        break;
                    case TransitionState.Resolve:
                        Resolve();
                        break;
                    case TransitionState.Stop:
                        Stop();
                        break;
                    case TransitionState.Delay:
                        Delay();
                        break;
                }
            }
            OnUpdateCycleCompleted?.Invoke();
            _currentCycle = 0;
        }
        /// <summary>
        /// Signals a pause in the update cycle, triggering the <see cref="OnExitDelayStarted"/> to alert updating 
        /// systems that the state machine requests a pause in updating for x amount of seconds.
        /// </summary>
        private void Delay()
        {
            OnExitDelayStarted?.Invoke(_currentState.GetExitDelay(DataInstance));
            _transitionState = TransitionState.Resolve;
            _updating = false;
        }
        /// <summary>
        /// Provides pause in the update cycle.
        /// </summary>
        private void Stop()
        {
            if (_currentState == _nextState)
            {
                _transitionState = TransitionState.Update;
            }
            else
            {
                _transitionState = TransitionState.Exit;
            }
            _updating = false;
        }
        /// <summary>
        /// Get the <see cref="_nextState"/> value and decide whether to end the current update cycle.
        /// </summary>
        private void Resolve()
        {
            _nextState = _currentState.GetNext(DataInstance);
            if (_nextState == _currentState)
            {
                _transitionState = TransitionState.Stop;
            }
            else
            {
                if (_currentState.IsRepeater || _currentState.HasExitDelay)
                {
                    _transitionState = TransitionState.Exit;
                }
                else if (ShouldStopUpdating())
                {
                    _transitionState = TransitionState.Stop;
                }
                else
                {
                    _transitionState = TransitionState.Exit;
                }
            }
        }

        /// <summary>
        /// Enter state, execute OnEnter actions and trigger <see cref="OnStateEntered"/>
        /// </summary>
        /// <param name="state"></param>
        private void EnterState()
        {
            _currentState.OnEnter(DataInstance);
            OnStateEntered?.Invoke(_currentState);
            _transitionState = TransitionState.Update;
        }
        /// <summary>
        /// Execute OnUpdate actions and trigger <see cref="OnStateUpdated"/>
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        private void UpdateState()
        {
            _currentState.OnUpdate(DataInstance);
            OnStateUpdated?.Invoke(_currentState);
            if (_currentState.HasExitDelay)
            {
                _transitionState = TransitionState.Delay;
            }
            else
            {
                _transitionState = TransitionState.Resolve;
            }
        }
        /// <summary>
        /// Returns true if state machine has reached the end of an update cycle,false if state machine should continue stepping forward
        /// </summary>
        /// <returns></returns>
        private bool ShouldStopUpdating()
        {
            return _currentState.StateType == StateType.Cycle || _currentState.StateType == StateType.Leaf;
        }
        /// <summary>
        /// Execute OnExit actions, set <see cref="_nextState"/>, and trigger <see cref="OnStateExited"/>
        /// </summary>
        /// <param name="newState">State to transition to</param>
        private void ExitState()
        {
            _currentState.OnExit(DataInstance);
            var previousState = _currentState;
            _currentState = _nextState;
            OnStateExited?.Invoke(previousState);
            _transitionState = TransitionState.Enter;
        }
        /// <summary>
        /// <see cref="OnActionExecuted"/> trigger, which is used by <see cref="State"/>s everytime an action is executed.
        /// </summary>
        /// <param name="actionName"></param>
        public void TriggerActionExecutedEvent(string actionName) => OnActionExecuted?.Invoke(actionName);
        public override string ToString()
        {
            return $"Current State : {_currentState?.Name}";
        }
    }
}
