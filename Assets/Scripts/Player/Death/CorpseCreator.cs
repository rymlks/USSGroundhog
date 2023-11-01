using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Player.Death
{
    public class CorpseCreator : MonoBehaviour
    {
        private GameObject _ragdollPrefab;
        private GameObject _dyingBeingPrefab;
        private GameObject _frozenCorpsePrefab;
        private GameObject _frozenCorpseCrouchedPrefab;
        private GameObject _bloatedCorpsePrefab;
        private Transform _transformAtDeath;

        protected void Initialize(GameObject ragdollPrefab,
            GameObject dyingBeingPrefab, Transform transformAtDeath)
        {
            this._ragdollPrefab = ragdollPrefab;
            this._dyingBeingPrefab = dyingBeingPrefab;
            this._transformAtDeath = transformAtDeath;
            this._frozenCorpsePrefab = Resources.Load<GameObject>("FrozenCorpse");
            this._frozenCorpseCrouchedPrefab = Resources.Load<GameObject>("FrozenCorpseCrouched");
            this._bloatedCorpsePrefab = Resources.Load<GameObject>("BloatedCorpse");
        }

        public GameObject CreateCorpse(DeathCharacteristics deathCharacteristics, GameObject ragdollPrefab,
            GameObject dyingBeingPrefab, Transform transformAtDeath)
        {
            this.Initialize(ragdollPrefab, dyingBeingPrefab, transformAtDeath);
            
            if (deathCharacteristics.shouldProduceCorpse() && !deathCharacteristics.shouldBurnCorpse())
            {
                GameObject corpse;
                if (deathCharacteristics.shouldProduceRagdollCorpse())
                {
                    corpse = Instantiate(_ragdollPrefab);
                    minimizeMass(corpse);
                }
                else if (deathCharacteristics.shouldProduceBloatedCorpse())
                {
                    corpse = Instantiate(_bloatedCorpsePrefab);
                }
                else if (deathCharacteristics.shouldProduceRigidCorpse())
                {
                    corpse = Instantiate(deathCharacteristics.shouldProduceCrouchedCorpse() ? _frozenCorpseCrouchedPrefab : _frozenCorpsePrefab);
                }
                else
                {
                    corpse = Instantiate(_dyingBeingPrefab);
                }

                if (deathCharacteristics.shouldPropelCorpse())
                {
                    corpse.GetComponentInChildren<Rigidbody>().AddForce(deathCharacteristics.getExplosionStrength());
                }

                corpse.transform.position = _transformAtDeath.position;
                corpse.transform.rotation = _transformAtDeath.rotation;
                corpse.tag = "Corpse";
                return corpse;
            }

            return null;
        }

        protected void minimizeMass(GameObject toEnlighten)
        {
            foreach (var rb in toEnlighten.GetComponentsInChildren<Rigidbody>())
            {
                rb.mass = 0.001f;
            }
        }
    }
}