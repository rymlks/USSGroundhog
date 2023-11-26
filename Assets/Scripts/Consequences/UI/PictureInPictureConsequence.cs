#nullable enable
using Triggers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Consequences.UI
{
    public class PictureInPictureConsequence : AbstractInterruptibleConsequence
    {
        public Transform objectToLookAt;
        public Vector3 positionOffsetDirection = Vector3.forward;
        public float distance = 3f;
        public float duration = 2f;
        public Camera cameraToUse;

        void Start()
        {
            if (objectToLookAt == null)
            {
                objectToLookAt = GameObject.FindWithTag("Player").transform;
            }

            if (cameraToUse == null)
            {
                cameraToUse = GameObject.Find("PictureInPictureCamera").GetComponent<Camera>();
                if (cameraToUse == null)
                {
                    Debug.Log("Picture in Picture Consequence failed, no camera found.");
                    Destroy(this);
                }
            }
            
        }

        void Update()
        {
            if(startedAndElapsed())
            {
                turnOff();
            }
        }
        
        public override void Interrupt(TriggerData? data)
        {
            base.Interrupt(data);
            turnOff();
        }

        public override void Execute(TriggerData? data)
        {
            base.Execute(data);
            if (objectToLookAt != null)
            {
                turnOn();
            }
        }

        protected bool startedAndElapsed()
        {
            return started && Time.time - _startTime > duration;
        }

        protected void turnOff()
        {
            cameraToUse.enabled = false;
            started = false;
        }

        protected void turnOn()
        {
            cameraToUse.transform.position =
                objectToLookAt.position;
            cameraToUse.transform.SetParent(objectToLookAt);
            cameraToUse.transform.Translate(positionOffsetDirection.normalized * distance, Space.Self);
            cameraToUse.transform.LookAt(objectToLookAt);
            cameraToUse.enabled = true;
        }
    }
}
