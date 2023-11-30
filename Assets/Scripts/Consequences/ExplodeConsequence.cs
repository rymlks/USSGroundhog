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
        public bool disappear = false;
        public bool corpseShouldRagdoll = true;
        public bool produceCorpse = true;
        public GameObject ExplosionPrefab;

        public bool dontRespawn = false;
        protected bool die = false;
        protected bool _disappear = false;

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

            if (data.triggeringObject.CompareTag("Player"))
            {
                GameManager.instance.CommitDie(new Dictionary<string, object>()
                {
                    {"explosion", explosionVector3(data).normalized * explosionStrength},
                    {"ragdoll", corpseShouldRagdoll},
                    {"nocorpse", !produceCorpse},
                    {"dontRespawn", dontRespawn}
                });
            }

            if (!persist)
            {
                die = true;
            }

            if (disappear)
            {
                _disappear = true;
            }

            if (!data.triggeringObject.CompareTag("Player"))
            {
                data.triggeringObject.GetComponent<Rigidbody>().velocity = explosionVector3(data).normalized * explosionStrength;
            }
        }

        public void LateUpdate()
        {
            if (die)
            {
                Destroy(gameObject);
            }

            if (_disappear)
            {
                /*
                foreach (var renderer in GetComponents<MeshRenderer>())
                {
                    renderer.enabled = false;
                }
                foreach (var collider in GetComponents<Collider>())
                {
                    collider.enabled = false;
                }
                */
                gameObject.SetActive(false);
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