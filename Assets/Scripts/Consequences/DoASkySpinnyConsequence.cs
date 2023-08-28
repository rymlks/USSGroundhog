using System;
using StaticUtils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Consequences
{
    public class DoASkySpinnyConsequence : DoASpinnyConsequence
    {
        private float rotato = 0;


        void FixedUpdate()
        {
            if (started)
            {
                rotato += rotationScalar;
                if (Mathf.Abs(rotato) >= Mathf.Abs(max))
                {
                    rotato = max;
                }

                RenderSettings.skybox.SetFloat("_Rotation", rotato);
            }
        }
    }
}
