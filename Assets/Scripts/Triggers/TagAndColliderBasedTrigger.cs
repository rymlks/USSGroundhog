using System.Collections.Generic;
using System.Linq;
using Managers;
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
        protected HashSet<Collider> entered;
        public bool onTrigger = true;
        public bool onCollision = false;
        public bool onParticle = false;

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
        
        public virtual void OnParticleCollision(GameObject other)
        {
            if (this.RespondsToParticles())
            {
                Debug.Log("Particle collided from " + other.name);
                OnParticleEnter(other);
            }
        }

        protected void OnEnter(Collider other)
        {
            if (enabled &&
                this.isRelevantObject(other.gameObject))
            {
                if (!meetsRequirements())
                {
                    return;
                }

                initializeTrackedColliders();

                beginTrackingCollider(other);

                Engage(new TriggerData(CustomTag + " contacted", other.transform.position, other.gameObject));
            }
        }
        
        protected void OnParticleEnter(GameObject withParticleSystem)
        {
            if (enabled &&
                this.isRelevantObject(withParticleSystem))
            {
                if (!meetsRequirements())
                {
                    return;
                }

                Engage(new TriggerData(CustomTag + " contacted via particle", withParticleSystem.transform.position, withParticleSystem));
            }
        }


        protected override bool meetsRequirements()
        {
            if (!base.meetsRequirements())
            {
                return false;
            }
            if (RequireItem != "" && !GameManager.instance.getInventory().IsItemPossessed(RequireItem))
            {
                this.keyUI.ShowNextFrame();
                return false;
            }

            return true;
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
        
        public virtual bool RespondsToParticles()
        {
            return this.onParticle;
        }
    }
}