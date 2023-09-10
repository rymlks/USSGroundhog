using UnityEngine;

namespace Triggers
{
    public class TriggerData
    {
        public string triggerDescription;
        public Vector3? triggerLocation;
        public GameObject? triggeringObject;

        public TriggerData(string name)
        {
            this.triggerDescription = name;
            this.triggerLocation = null;
        }
        
        public TriggerData(string name, Vector3 location)
        {
            this.triggerDescription = name;
            this.triggerLocation = location;
        }

        public TriggerData(string name, Vector3 location, GameObject triggeringObject)
        {
            this.triggerDescription = name;
            this.triggerLocation = location;
            this.triggeringObject = triggeringObject;
        }
    }
}