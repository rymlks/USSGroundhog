using System.Collections.Generic;
using System.Linq;
using StaticUtils;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using static StaticUtils.UnityUtil;

namespace Triggers
{
    public class TriggerOnTagEnter : TagBasedTrigger
    {
        public string RequireItem = "";
        private KeyStatusUIController keyUI;
        public bool reEngageOnStay = true;
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
            if (enabled &&
                this.intersectingRelevantObject(other))
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