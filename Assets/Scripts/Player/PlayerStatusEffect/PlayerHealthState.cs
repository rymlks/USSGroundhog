using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UI;
using UnityEngine;

public class PlayerHealthState : MonoBehaviour
{
    
    public interface ICurableStatus
    {
        void Cure();
    }
    
    protected List<StatusTracker> statusTrackers;

    public class StatusTracker : ICurableStatus
    {
        public float secondsUntilCritical;
        public float secondsMaximumCapacity;
        public bool shouldCancelNextRecovery = false;
        public string statusEffectName;
        public float recoverySpeedFactor = 2;
        public StatusEffectUIController uiController;
        public bool worsensUntilCured;
        private bool _isCurrentlyBeingInflicted;

        public StatusTracker(string name, float secondsMaximum, bool worsens = false)
        {
            this.statusEffectName = name;
            this.secondsMaximumCapacity = secondsMaximum;
            this.uiController = StatusEffectUIController.GetByStatusName(name);
            this.worsensUntilCured = worsens;
            this._isCurrentlyBeingInflicted = false;
            Reset();
        }

        public void recover(float deltaTime)
        {
            if (shouldCancelNextRecovery)
            {
                shouldCancelNextRecovery = false;
            }
            else
            {
                this.secondsUntilCritical = Mathf.Min(secondsMaximumCapacity,
                    secondsUntilCritical + deltaTime / recoverySpeedFactor);
            }
        }

        public void suffer(float deltaSeconds)
        {
            this._isCurrentlyBeingInflicted = true;
            this.secondsUntilCritical = Mathf.Max(0f, this.secondsUntilCritical - deltaSeconds);
            if (secondsUntilCritical <= 0f)
            {
                GameManager.instance.CommitDie(statusEffectName);
                this.Reset();
            }
            this.uiController.ShowNextFrame();
            shouldCancelNextRecovery = true;
        }

        public void Reset()
        {
            this.secondsUntilCritical = secondsMaximumCapacity;
            this.shouldCancelNextRecovery = false;
            this._isCurrentlyBeingInflicted = false;
        }

        public bool WorsensUntilCured()
        {
            return worsensUntilCured;
        }

        public bool IsActive()
        {
            return worsensUntilCured ? this._isCurrentlyBeingInflicted : true;
        }

        public void Cure()
        {
            if (!worsensUntilCured)
            {
                return;
            }
            else
            {
                this._isCurrentlyBeingInflicted = false;
                this.Reset();
            }
        }
    }

    void Start()
    {
        ResetAllStatuses();
    }

    public void ResetAllStatuses()
    {
        statusTrackers = new List<StatusTracker>();
        statusTrackers.Add(new StatusTracker("suffocation", 4.5f));
        statusTrackers.Add(new StatusTracker("freezing", 3f));
        statusTrackers.Add(new StatusTracker("burning", 2f));
        statusTrackers.Add(new StatusTracker("bleeding", 10f, true));
    }

    void Update()
    {
        foreach(StatusTracker tracker in statusTrackers){
            if (tracker.WorsensUntilCured() && tracker.IsActive())
            {
                tracker.suffer(Time.deltaTime);
            }
            else
            {
                tracker.recover(Time.deltaTime);
            }
        }
    }
    
    public void Hurt(string damageSourceName, float deltaSeconds)
    {
        statusTrackers.Find(tracker => tracker.statusEffectName == damageSourceName).suffer(deltaSeconds);
    }

    public void Cure(string damageSourceName)
    {
        statusTrackers.Find(tracker => tracker.statusEffectName == damageSourceName).Cure();
    }
}
