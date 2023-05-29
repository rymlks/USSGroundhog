using System.Collections.Generic;
using System.Linq;
using StaticUtils;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using static StaticUtils.UnityUtil;

namespace Triggers
{
    public abstract class TagAndColliderBasedTrigger : TagBasedTrigger, IColliderBasedTrigger
    {
        public string RequireItem = "";
        private KeyStatusUIController keyUI;
        public bool reEngageOnStay = true;
        public bool reverseOnExit = false;
        public Behaviour[] requireComponentsEnabled;
        protected HashSet<Collider> entered;
        public bool onTrigger = true;
        public bool onCollision = false;

        protected virtual void Start()
        {
            base.Start();
            if (keyUI == null)
            {
                keyUI = FindObjectOfType<KeyStatusUIController>();
            }

            entered = new HashSet<Collider>();
        }


        public virtual void OnTriggerEnter(Collider other)
        {
            if (this.RespondsToTriggers())
            {
                OnEnter(other);
            }
        }

        public virtual void OnTriggerExit(Collider other)
        {
            if (this.RespondsToTriggers())
            {
                OnExit(other);
            }
        }

        public virtual void OnTriggerStay(Collider other)
        {
            if (this.RespondsToTriggers())
            {
                OnStay(other);
            }
        }

        public virtual void OnCollisionEnter(Collision other)
        {
            if (this.RespondsToColliders())
            {
                OnEnter(other.collider);
            }
        }

        public virtual void OnCollisionExit(Collision other)
        {
            if (this.RespondsToColliders())
            {
                OnExit(other.collider);
            }
        }

        public virtual void OnCollisionStay(Collision other)
        {
            if (this.RespondsToColliders())
            {
                OnStay(other.collider);
            }
        }

        protected void OnEnter(Collider other)
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

                initializeTrackedColliders();

                beginTrackingCollider(other);
                
                Engage(new TriggerData(CustomTag + " contacted", other.transform.position));
                if (destroy)
                {
                    Destroy(gameObject);
                }
            }
        }

        private void beginTrackingCollider(Collider other)
        {
            if (!entered.Contains(other))
            {
                entered.Add(other);
            }
        }

        private void initializeTrackedColliders()
        {
            if (entered == null)
            {
                entered = new HashSet<Collider>();
            }
        }


        protected void OnStay(Collider other)
        {
            if (reEngageOnStay)
            {
                OnEnter(other);
            }
        }


        protected void OnExit(Collider other)
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

        public virtual bool RespondsToTriggers()
        {
            return this.onTrigger;
        }

        public virtual bool RespondsToColliders()
        {
            return this.onCollision;
        }
    }
}