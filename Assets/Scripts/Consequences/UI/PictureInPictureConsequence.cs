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
            }
        }

        void Update()
        {
            if(started && Time.time - _startTime > duration)
            {
                cameraToUse.enabled = false;
                started = false;
            }
        }

        public override void Execute(TriggerData? data)
        {
            base.Execute(data);
            if (objectToLookAt != null)
            {
                cameraToUse.transform.position =
                    objectToLookAt.position;
                cameraToUse.transform.SetParent(objectToLookAt);
                cameraToUse.transform.Translate( positionOffsetDirection.normalized * distance, Space.Self);
                cameraToUse.transform.LookAt(objectToLookAt);
                cameraToUse.enabled = true;
            }
        }
    }
}
