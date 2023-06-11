#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Triggers;
using UnityEngine;

namespace Consequences
{
    public class ExecuteConsequencesByObjectName : AbstractConsequence
    {
        [SerializeField] [Tooltip("Object names to find and execute consequences on.")]
        protected string[] objectNames;

        public override void Execute(TriggerData? data)
        {
            foreach (var consequenceObjectName in objectNames)
            {
                foreach (var consequence in GameObject.Find(consequenceObjectName).GetComponents<AbstractConsequence>())
                {
                    consequence.Execute(data);
                }
            }
        }
    }
}