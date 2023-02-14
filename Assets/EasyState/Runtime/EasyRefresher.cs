// Developed by Pigeon Studios
// Released with EasyState a Unity Asset
// For support email pigeonstudios.dev@gmail.com
// Documentation https://thepigeonfighter.github.io/EasyState/
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using EasyState.Core;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace EasyState
{
    /// <summary>
    /// Tracks and updates all EasyStateMachines that have a defined
    /// refresh type.
    /// </summary>
    [AddComponentMenu("Pigeon Studios/EasyState/Easy Refresher")]
    public class EasyRefresher : MonoBehaviour
    {
        /// <remarks>
        /// This is used by the EasyRefresher Inspector component to display pertinent stats in the Editor.
        /// </remarks>

        #region EasyRefresher Stats

        public int BackgroundWithEventSyncCount;
        public int BackgroundRefresherCount;
        public int UpdateRefresherCount;
        public int LateUpdateRefreshCount;
        public int FixedUpdateRefreshCount;
        public int CustomRateMachineCount;
        public int TotalRefreshCount => BackgroundWithEventSyncCount + BackgroundRefresherCount + UpdateRefresherCount + FixedUpdateRefreshCount + LateUpdateRefreshCount + CustomRateMachineCount;

        #endregion EasyRefresher Stats

        #region Refresh Queues

        /// <summary>
        /// A queue of StateMachines that have finished updating in the background
        /// and are ready to has the PostUpdateEvent called.
        /// </summary>
        private readonly Queue<CustomRateMachine> _backgroundUpdateFinishedQueue = new Queue<CustomRateMachine>();
        private readonly LinkedList<CustomRateMachine> _customRateMachines = new LinkedList<CustomRateMachine>();
        #endregion Refresh Queues

        #region Unity Message Driven Update

        private readonly LinkedList<Machine> _fixedUpdateMachines = new LinkedList<Machine>();
        private readonly LinkedList<Machine> _lateUpdateMachines = new LinkedList<Machine>();
        private readonly LinkedList<Machine> _updateMachines = new LinkedList<Machine>();
        private float _unityTime;
        #endregion Unity Message Driven Update

        #region EasyRefresher Singleton Instance

        private static EasyRefresher _instance;

        /// <summary>
        /// The single instance that all EasyStateMachines use to register themselves to.
        /// </summary>
        public static EasyRefresher Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<EasyRefresher>();
                    if (_instance == null)
                    {
                        GameObject gb = new GameObject("EasyState Global Refresher");
                        _instance = gb.AddComponent<EasyRefresher>();
                    }
                }
                return _instance;
            }
        }

        #endregion EasyRefresher Singleton Instance

        public CustomRateMachine AddBackgroundRefresher(IStateMachine machine, IEventController eventController, float cycleTime)
        {
            BackgroundRefresherCount++;
            CustomRateMachine customMachine = BuildCustomRateMachine(machine, eventController, cycleTime);
            customMachine.backgroundThread = true;
            customMachine.isRunning = true;
            _customRateMachines.AddLast(customMachine);
            return customMachine;
        }

        public CustomRateMachine AddBackgroundWithEventSync(IStateMachine machine, IEventController eventController, float cycleTime)
        {
            BackgroundWithEventSyncCount++;
            CustomRateMachine customMachine = BuildCustomRateMachine(machine, eventController, cycleTime);
            customMachine.backgroundThread = true;
            customMachine.hasEventSync = true;
            _customRateMachines.AddLast(customMachine);
            return customMachine;
        }

        public CustomRateMachine AddCustomRateRefresher(IStateMachine machine, IEventController eventController, float cycleTime)
        {
            CustomRateMachineCount++;
            CustomRateMachine customMachine = BuildCustomRateMachine(machine, eventController, cycleTime);
            _customRateMachines.AddLast(customMachine);
            return customMachine;
        }
        private static CustomRateMachine BuildCustomRateMachine(IStateMachine machine, IEventController eventController, float cycleTime)
        {
            CustomRateMachine customMachine = new CustomRateMachine
            {
                machine = machine,
                eventController = eventController,
                cycleRate = cycleTime,
                isRunning = true,
                nextUpdateTime = Time.realtimeSinceStartup
            };
            customMachine.machine.OnExitDelayStarted += (delayAmount) => customMachine.nextUpdateTime += delayAmount;
           
            return customMachine;
        }
        private static Machine BuildMachine(IStateMachine machine, IEventController eventController)
        {
            Machine pair = new Machine
            {
                machine = machine,
                eventController = eventController
            };
            machine.OnExitDelayStarted += (delayTime) => pair.delayEndTime = Time.realtimeSinceStartup + delayTime;
            return pair;
        }
        public Machine AddFixedUpdateRefresher(IStateMachine machine, IEventController eventController)
        {
            FixedUpdateRefreshCount++;
            Machine pair = BuildMachine(machine, eventController);
            _fixedUpdateMachines.AddLast(pair);
            return pair;
        }


        public Machine AddLateUpdateRefresher(IStateMachine machine, IEventController eventController)
        {
            LateUpdateRefreshCount++;
            Machine pair = BuildMachine(machine, eventController);
            _lateUpdateMachines.AddLast(pair);
            return pair;
        }

        public Machine AddUpdateRefresher(IStateMachine machine, IEventController eventController)
        {
            UpdateRefresherCount++;
            Machine pair = BuildMachine(machine, eventController);
            _updateMachines.AddLast(pair);
            return pair;
        }


        public void StopCustomRateMachine(CustomRateMachine machine)
        {
            machine.isRunning = false;
            _customRateMachines.Remove(machine);
            if (machine.backgroundThread && machine.hasEventSync)
            {
                BackgroundWithEventSyncCount--;
            }
            else if (machine.backgroundThread)
            {
                BackgroundRefresherCount--;
            }
            else
            {
                CustomRateMachineCount--;
            }
        }

        public void StopFixedUpdateRefresher(Machine machine)
        {
            _fixedUpdateMachines.Remove(machine);
            FixedUpdateRefreshCount--;
        }

        public void StopLateUpdateRefresher(Machine machine)
        {
            _lateUpdateMachines.Remove(machine);
            LateUpdateRefreshCount--;
        }

        public void StopUpdateRefresher(Machine machine)
        {
            _updateMachines.Remove(machine);
            UpdateRefresherCount--;
        }
        private void ExecuteBackgroundUpdate(CustomRateMachine machine)
        {
            try
            {
                machine.machine.Update();
                lock (_backgroundUpdateFinishedQueue)
                {
                    _backgroundUpdateFinishedQueue.Enqueue(machine);
                }

            }
            catch (System.Exception e)
            {
                var message = e.Message;
            }
        }

        private void FixedUpdate()
        {
            foreach (var node in _fixedUpdateMachines)
            {
                UpdateMachine(node);
            }
        }

        private void LateUpdate()
        {
            foreach (var node in _lateUpdateMachines)
            {
                UpdateMachine(node);
            }
        }

        private void OnDisable()
        {

            _instance = null;
        }

#if UNITY_EDITOR

        private void Reset()
        {
            RemoveIfOtherRefreshersExist();
        }

        private void RemoveIfOtherRefreshersExist()
        {
            var otherInstances = Resources.FindObjectsOfTypeAll<EasyRefresher>();
            if (otherInstances.Length > 1)
            {
                string name = "";
                foreach (var other in otherInstances)
                {
                    if (other == this) continue;
                    name = other.gameObject.name;
                    break;
                }
                Debug.LogWarning($"Cannot have more than one EasyRefresher component in the same scene. {name}(GameObject) already has a EasyRefresher component attached to it.");
                DestroyImmediate(this);
            }
        }


#endif

        private void Update()
        {
            _unityTime = Time.realtimeSinceStartup;
            foreach (var node in _updateMachines)
            {
                UpdateMachine(node);
            }
            foreach (var node in _customRateMachines)
            {
                UpdateMachine(node);
            }
            int bgQueueCount = _backgroundUpdateFinishedQueue.Count;
            if (bgQueueCount > 0)
            {
                while (bgQueueCount > 0)
                {
                    bgQueueCount--;
                    var customMachine = _backgroundUpdateFinishedQueue.Dequeue();
                    customMachine.eventController.OnPostUpdate(customMachine.machine.DataBaseInstance);
                }
            }
        }

        private void UpdateMachine(Machine machine)
        {
            if (machine.canUpdate)
            {
                machine.eventController.OnPreUpdate(machine.machine.DataBaseInstance);
                machine.machine.Update();
                machine.eventController.OnPostUpdate(machine.machine.DataBaseInstance);
            }
        }

        private void UpdateMachine(CustomRateMachine machine)
        {
            if ((_unityTime < machine.nextUpdateTime && machine.isRunning))
            {
                return;
            }
            machine.nextUpdateTime = _unityTime + machine.cycleRate;
            if (!machine.backgroundThread)
            {
                machine.eventController.OnPreUpdate(machine.machine.DataBaseInstance);
                machine.machine.Update();
                machine.eventController.OnPostUpdate(machine.machine.DataBaseInstance);
            }
            else
            {
                if (machine.hasEventSync)
                {
                    machine.eventController.OnPreUpdate(machine.machine.DataBaseInstance);
                    ThreadPool.QueueUserWorkItem((x) => ExecuteBackgroundUpdate(machine));
                }
                else
                {
                    ThreadPool.QueueUserWorkItem((x) => machine.machine.Update());
                }
            }
        }
        private enum CustomMachineType { }
        /// <summary>
        /// Represents a machine that is being updated by a custom timer.
        /// </summary>
        public class CustomRateMachine
        {
            public IEventController eventController;
            public IStateMachine machine;
            //public PausableTimer timer;
            public float nextUpdateTime;
            public float cycleRate;
            public bool backgroundThread;
            public bool hasEventSync;
            public bool isRunning;
        }

        /// <summary>
        /// Represents a simple machine that is being updated by Unity Message methods.
        /// </summary>
        public class Machine
        {
            public IEventController eventController;
            public IStateMachine machine;
            public float delayEndTime;
            public bool canUpdate => Time.realtimeSinceStartup > delayEndTime;
        }
    }
}