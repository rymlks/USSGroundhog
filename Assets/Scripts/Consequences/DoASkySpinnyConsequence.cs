using System;
using StaticUtils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Consequences
{
    public class DoASkySpinnyConsequence : DoASpinnyConsequence
    {
        private float rotatoo = 0;
        protected float initialRotation = -1f;

        protected override void Start()
        {
            base.Start();
            initialRotation = RenderSettings.skybox.GetFloat("_Rotation");
        }

        void FixedUpdate()
        {
            if (started)
            {
                rotatoo += rotationScalar;
                if (Mathf.Abs(rotatoo) >= Mathf.Abs(max))
                {
                    rotatoo = max;
                }

                RenderSettings.skybox.SetFloat("_Rotation", rotatoo);
            }
        }

        protected void OnDestroy()
        {
            RenderSettings.skybox.SetFloat("_Rotation", initialRotation);
        }
    }
}