using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Triggers
{
    public class TriggerOnTagEnter : AbstractTrigger
    {
        public string CustomTag = "";
        public bool acceptTagInParent = false;
        public string RequireItem = "";
        private KeyStatusUIController keyUI;
        [FormerlySerializedAs("OnStay")] public bool reEngageOnStay = true;
        public bool reverseOnExit = false;
        public Behaviour[] requireComponentsEnabled;
        protected HashSet<Collider> entered;

        new void Start()
        {
            base.Start();
            if (keyUI == null)
            {
                keyUI = FindObjectOfType<KeyStatusUIController>();
            }

            entered = new HashSet<Collider>();
        }

        
        
        public void OnTriggerEnter(Collider other)
        {
            if ( enabled && 
                (CustomTag == "" || 
                 other.CompareTag(CustomTag)))
            {
                if (RequireItem != "" && !GameManager.instance.getInventory().IsItemPossessed(RequireItem))
                {
                    this.keyUI.ShowNextFrame();
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
                entered.Add(other);
                if (destroy)
                {
                    Destroy(gameObject);
                }
            }
        }

        public void OnTriggerStay(Collider other)
        {
            if (reEngageOnStay)
            {
                OnTriggerEnter(other);
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (entered.Contains(other))
            {
                if (reverseOnExit)
                {
                    this.Disengage(new TriggerData(CustomTag + " left contact", other.transform.position));
                }
                entered.Remove(other);
            }
        }
    }
}
