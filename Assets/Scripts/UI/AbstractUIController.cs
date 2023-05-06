using UnityEngine;

namespace UI
{
    public abstract class AbstractUIController : MonoBehaviour, IUIController
    {
        protected float lastAffectedTime = float.NegativeInfinity;
        public float secondsMessagePersists = 1f;
        
        protected virtual void LateUpdate()
        {
            if (this.shouldShowNow())
            {
                EnableUI();
            }
            else
            {
                DisableUI();
            }
        }
        
        public virtual void ShowNextFrame()
        {   
            this.lastAffectedTime = Time.time;
        }
        
        protected virtual bool shouldShowNow()
        {
            float timeSinceLastAffected = Time.time - this.lastAffectedTime;
            return timeSinceLastAffected >= 0 && timeSinceLastAffected < secondsMessagePersists;
        }

        protected abstract void EnableUI();
        
        protected abstract void DisableUI();
    }
}