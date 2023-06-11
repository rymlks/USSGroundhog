#nullable enable
using System;
using System.Collections.Generic;
using Managers;
using Triggers;
using UnityEngine;

namespace Consequences
{
    public class ExplodeConsequence : AbstractConsequence
    {
        public float explosionStrength = 10;
        public bool persist = true;
        public bool corpseShouldRagdoll = true;
        public GameObject ExplosionPrefab;

        public bool dontRespawn = false;
        protected bool die = false;

        protected void instantiateExplosion()
        {
            if (this.ExplosionPrefab != null)
            {
                var explode = Instantiate(ExplosionPrefab);
                explode.transform.position = transform.position;
            }
        }

        public override void Execute(TriggerData? data)
        {
            instantiateExplosion();
        
            GameManager.instance.CommitDie(new Dictionary<string, object>()
            {
                {"explosion", explosionVector3(data).normalized * explosionStrength},
                {"ragdoll", corpseShouldRagdoll},
                {"dontRespawn", dontRespawn}
            });
            if (!persist)
            {
                die = true;
            }
        }

        public void LateUpdate()
        {
            if (die)
            {
                Destroy(gameObject);
            }
        }

        private Vector3 explosionVector3(TriggerData? data)
        {
            if (data != null)
            {
                return (Vector3) (data.triggerLocation - transform.position);
            }
            else
            {
                return transform.position;
            }
        }
    }
}