#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Triggers;
using UnityEngine;

namespace Consequences
{
    public class ToggleGameObjectsConsequence : AbstractCancelableConsequence
    {
        public GameObject[] toToggle;

        public void Start()
        {
            if (this.toToggle.Length < 1)
            {
                toToggle = new GameObject[]{this.gameObject};
            }
        }

        public override void Execute(TriggerData? data)
        {
            foreach (GameObject gobject in toToggle)
            {
                gobject.SetActive(!gobject.activeInHierarchy);
            }
        }
        
        public override void Cancel(TriggerData? data)
        {
            Execute(data);
        }
    }
}