using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PlayerStatusEffect;
using UnityEngine;

public class PlayerHealthState : MonoBehaviour
{
    protected List<StatusTracker> statusTrackers;

    public class StatusTracker
    {
        public float secondsUntilCritical;
        public float secondsMaximumCapacity;
        public bool shouldCancelNextRecovery = false;
        public string statusEffectName;
        public float recoverySpeedFactor = 2;
        public StatusEffectUIController uiController;
        public bool worsensUntilCured;

        public StatusTracker(string name, float secondsMaximum, bool worsens = false)
        {
            this.statusEffectName = name;
            this.secondsMaximumCapacity = secondsMaximum;
            this.uiController = FindObjectsOfType<StatusEffectUIController>().First(controller => controller.statusName == this.statusEffectName);
            this.worsensUntilCured = worsens;
            reset();
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
            this.secondsUntilCritical = Mathf.Max(0f, this.secondsUntilCritical - deltaSeconds);
            if (secondsUntilCritical <= 0f)
            {
                GameManager.instance.CommitDie(statusEffectName);
            }
            this.uiController.showStatusNextFrame();
            shouldCancelNextRecovery = true;
        }

        public void reset()
        {
            this.secondsUntilCritical = secondsMaximumCapacity;
            this.shouldCancelNextRecovery = false;
        }

        public bool WorsensUntilCured()
        {
            return worsensUntilCured;
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
    }

    void Update()
    {
        foreach(StatusTracker tracker in statusTrackers){
            if (tracker.WorsensUntilCured())
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
}
