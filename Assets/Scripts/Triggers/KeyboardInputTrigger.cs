using System.Linq;
using StaticUtils;
using UnityEngine;

namespace Triggers
{
    public class KeyboardInputTrigger : AbstractTrigger
    {
        public KeyCode activatingKey = KeyCode.E;
        public float maxDistance = 5f;

        void Update()
        {
            if (playerIsMakingInput() && playerIsWithinDistance() && playerIsLookingAtThis())
            {
                this.Engage();
            }
        }
        
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

        protected bool playerIsMakingInput()
        {
            return Input.GetKeyUp(activatingKey);
        }
    }
}