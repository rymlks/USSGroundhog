using UnityEngine;

namespace Consequences
{
    public class DoASpinnyConsequence : AbstractInterruptibleConsequence
    {
        public GameObject toSpin;
        public float speed;
        public float max = float.PositiveInfinity;
        public Vector3 axis = Vector3.up;

        protected Quaternion initialRotation;
    
        private float rotato = 0;

        void Start()
        {
            if (toSpin == null)
            {
                this.toSpin = this.gameObject;
            }

            if (max <= 0)
            {
                max = float.PositiveInfinity;
            }
            if(speed == 0){
                speed = 1;
            }

            this.initialRotation = this.toSpin.transform.localRotation;
        }


        void FixedUpdate()
        {
            if (started)
            {
                rotato += speed;
                if (Mathf.Abs(rotato) >= Mathf.Abs(max))
                {
                    rotato = max;
                }

                toSpin.transform.localRotation =
                    initialRotation * Quaternion.Euler(rotato * axis.x, rotato * axis.y, rotato * axis.z);
            }
        }
    }
}