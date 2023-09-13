using System.Linq;
using StaticUtils;
using UnityEngine;

namespace Triggers
{
    public class KeyboardInputTrigger : AbstractTrigger
    {
        public KeyCode activatingKey = KeyCode.E;
        public float maxDistance = 5f;


        protected bool playerIsLookingAtThis()
        {
            Transform cameraTransform = UnityUtil.getCameraTransform();
            return Physics.RaycastAll(cameraTransform.position, cameraTransform.forward, maxDistance + 10f).Any(hit => hit.transform.Equals(this.transform));
        }

        protected bool playerIsWithinDistance()
        {
            var isWithinDistance = maxDistance > Vector3.Distance(this.gameObject.transform.position, UnityUtil.getPlayerPosition());
            return isWithinDistance;
        }

        void Update()
        {
            if (Input.GetKeyUp(activatingKey) && playerIsWithinDistance() && playerIsLookingAtThis())
            {
                this.Engage();
            }
        }
    }
}