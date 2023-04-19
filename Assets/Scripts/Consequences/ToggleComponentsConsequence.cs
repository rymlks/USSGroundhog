#nullable enable
using Triggers;
using UnityEngine;

namespace Consequences
{
    public class ToggleComponentsConsequence : MonoBehaviour, IConsequence
    {
        public GameObject componentRoot;
        public string componentType = "light";
        public bool inChildren = true;

        public void Start()
        {
            if (this.componentRoot == null)
            {
                componentRoot = this.gameObject;
            }
        }

        public void execute(TriggerData? data)
        {
            if (componentType == "light" && inChildren)
            {
                foreach (Light currentLight in componentRoot.GetComponentsInChildren<Light>(true))
                {
                    currentLight.enabled = !currentLight.enabled;
                }
            }
        }
    }
}
