#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Triggers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Consequences
{
    public class EnableOrDisableGameObjectsConsequence : AbstractCancelableConsequence
    {
        public GameObject[] toEnable;
        public bool disableInstead = false;
        
        public void Start()
        {
            if (this.toEnable.Length < 1)
            {
                toEnable = new GameObject[]{this.gameObject};
            }
        }

        public override void Execute(TriggerData? data)
        {
            foreach (GameObject gobject in toEnable)
            {
                gobject.SetActive(!disableInstead);
            }
        }
        
        public override void Cancel(TriggerData? data)
        {
            foreach (GameObject gobject in toEnable)
            {
                gobject.SetActive(disableInstead);
            }
        }
    }
}