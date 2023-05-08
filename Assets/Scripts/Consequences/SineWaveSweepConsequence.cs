using UnityEngine;

namespace Consequences
{
    public class SineWaveSweepConsequence : AbstractInterruptibleConsequence
    {
        public Transform toRotate;
        public float angle;
        void Start()
        {
            if (toRotate == null)
            {
                toRotate = gameObject.transform;
            }
        }

        void Update()
        {
            if (this.started)
            {
                float sineWave = Mathf.Sin(Time.time);
                float clampedAngle = sineWave * angle;
                this.gameObject.transform.rotation =
                    Quaternion.Euler(toRotate.eulerAngles.x, toRotate.eulerAngles.y, clampedAngle);
            }
        }
    }
}
