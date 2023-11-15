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
        public bool disableOnly = false;
        public bool enableOnly = false;

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
                if (this.enableOnly)
                {
                    gobject.SetActive(true);
                } else if (this.disableOnly)
                {
                    gobject.SetActive(false);
                }
                else
                {
                    gobject.SetActive(!gobject.activeInHierarchy);
                }
            }
        }
        
        public override void Cancel(TriggerData? data)
        {
            foreach (GameObject gobject in toToggle)
            {
                if (this.enableOnly)
                {
                    gobject.SetActive(false);
                } else if (this.disableOnly)
                {
                    gobject.SetActive(true);
                }
                else
                {
                    gobject.SetActive(!gobject.activeInHierarchy);
                }
            }
        }
    }
}