using System.Collections.Generic;
using System.Linq;
using StaticUtils;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using static StaticUtils.UnityUtil;

namespace Triggers
{
    public class TriggerOnAnyCollision : TriggerOnTagIntersection
    {
        protected virtual void Start()
        {
            base.Start();
            this.onTrigger = false;
            this.onCollision = true;
        }

        protected override bool isRelevantObject(GameObject other)
        {
            return true;
        }
    }
}