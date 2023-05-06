using UI;
using UnityEngine;

namespace Triggers
{
    public class TriggerOnTagEnter : AbstractTrigger
    {
        public string CustomTag = "";
        public bool acceptTagInParent = false;
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
            if ( enabled && 
                (CustomTag == "" || 
                 other.CompareTag(CustomTag)))
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

                Engage(new TriggerData(CustomTag + " contacted", other.transform.position));
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
