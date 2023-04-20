using UnityEngine;

namespace Triggers
{
    public class TriggerOnTagEnter : AbstractTrigger
    {
        public string CustomTag = "";
        public string RequireItem = "";
        private KeyStatusUIController keyUI;
        public bool OnStay = true;
        public Behaviour[] requireComponentsEnabled;

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
            if (CustomTag == "" || other.CompareTag(CustomTag))
            {
                if (RequireItem != "" && !GameManager.instance.itemIsPossessed(RequireItem))
                {
                    this.keyUI.showStatusNextFrame();
                    return;
                }

                foreach (Behaviour compo in requireComponentsEnabled)
                {
                    if (!compo.isActiveAndEnabled)
                    {
                        return;
                    }
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
            if (OnStay)
            {
                OnTriggerEnter(other);
            }
        }
    }
}
