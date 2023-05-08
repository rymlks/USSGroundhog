#nullable enable
using System.Collections.Generic;
using Triggers;
using UnityEngine;

namespace Consequences
{
    public class ExplodeConsequence : AbstractConsequence
    {
        public float explosionStrength = 10;
        public bool persist = true;
        public GameObject ExplosionPrefab;

        public bool dontRespawn = false;

        public void Start()
        {
            this.GetComponent<Collider>().isTrigger = true;
        }

        protected void instantiateExplosion()
        {
            if (this.ExplosionPrefab != null)
            {
                var explode = Instantiate(ExplosionPrefab);
                explode.transform.position = transform.position;
            }
        }

        public override void execute(TriggerData? data)
        {
            instantiateExplosion();
        
            GameManager.instance.CommitDie(new Dictionary<string, object>()
            {
                {"explosion", explosionVector3(data).normalized * explosionStrength},
                {"ragdoll", true},
                {"dontRespawn", dontRespawn}
            });
            if (!persist)
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