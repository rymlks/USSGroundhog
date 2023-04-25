using UnityEngine;

namespace Triggers
{
    public class TriggerOnTagEnter : AbstractTrigger
    {
        public string CustomTag = "";
        public string RequireItem = "";
        public bool onStay = true;
        private KeyStatusUIController keyUI;

        new void Start()
        {
            base.Start();
            if (keyUI == null)
            {
                keyUI = FindObjectOfType<KeyStatusUIController>();
            }
        }

        public void OnTriggerEnter(Collider other)
        {
            if ( enabled && 
                (CustomTag == "" || 
                 other.CompareTag(CustomTag)))
            {
                if (RequireItem != "" && !GameManager.instance.itemIsPossessed(RequireItem))
                {
                    this.keyUI.showStatusNextFrame();
                    return;
                }

                Engage();
                if (destroy)
                {
                    Destroy(gameObject);
                }
            }
        }

        public void OnTriggerStay(Collider other)
        {
            if (onStay)
            {
                OnTriggerEnter(other);
            }
        }
    }
}
