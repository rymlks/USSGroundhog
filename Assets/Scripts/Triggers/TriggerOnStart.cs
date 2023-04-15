using UnityEngine;

namespace Triggers
{
    public class TriggerOnStart : AbstractTrigger
    {
        protected override void Start()
        {
            base.Start();
            this.Engage(new TriggerData(this.gameObject.name + " triggered consequences on start"));
        }
    }
}
