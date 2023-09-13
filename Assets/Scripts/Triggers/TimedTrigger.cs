#nullable enable
using System.Collections.Generic;
using System.Linq;
using Consequences;
using UnityEngine;

namespace Triggers
{
    public class TimedTrigger : AbstractTrigger
    {
        public float onTime = 1;
        public float offTime = 1;
        public bool repeat = true;
        
        protected float timeLastActivated;

        public void Start()
        {
            base.Start();
            this.timeLastActivated = Time.time;
        }

        protected bool isOn()
        {
            return timeLastActivated + onTime >= Time.time;
        }

        public void Update()
        {
            if (isOn())
            {
                this.Engage(new TriggerData("Timer engaged", this.transform.position));
            }
        }


    }
}
