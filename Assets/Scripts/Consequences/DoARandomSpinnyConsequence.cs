using System;
using StaticUtils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Consequences
{
    public class DoARandomSpinnyConsequence : DoASpinnyConsequence
    {
        protected override void Start()
        {
            base.Start();
            this.axis = new Vector3(Random.value, Random.value, Random.value);
        }
    }
}
