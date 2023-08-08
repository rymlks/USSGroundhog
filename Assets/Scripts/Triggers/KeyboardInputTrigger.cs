using UnityEngine;

namespace Triggers
{
    public class KeyboardInputTrigger : AbstractTrigger
    {
        public KeyCode activatingKey = KeyCode.E;
        public float maxDistance = 5f;


        protected bool playerIsLookingAtThis()
        {
            
        }

        protected bool playerIsWithinDistance()
        {
            return maxDistance > Vector3.Distance(this.gameObject.transform.position, )
        }

        void Update()
        {
            if (Input.GetKeyUp(activatingKey) && playerIsLookingAtThis() && playerIsWithinDistance())
            {
                
            }
        }
    }
}