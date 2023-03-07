using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Player.Death
{
    public class CorpseCreator : MonoBehaviour
    {
        private Dictionary<string, object> _deathCharacteristics;
        private GameObject _ragdollPrefab;
        private GameObject _dyingBeingPrefab;
        private Transform _transformAtDeath;
        private DeathCharacteristicsProcessor _death;

        protected void Initialize(Dictionary<string, object> deathCharacteristics, GameObject ragdollPrefab,
            GameObject dyingBeingPrefab, Transform transformAtDeath)
        {
            this._deathCharacteristics = deathCharacteristics;
            this._ragdollPrefab = ragdollPrefab;
            this._dyingBeingPrefab = dyingBeingPrefab;
            this._transformAtDeath = transformAtDeath;
            this._death = new DeathCharacteristicsProcessor(_deathCharacteristics);
        }

        public GameObject CreateCorpse(Dictionary<string, object> deathCharacteristics, GameObject ragdollPrefab,
            GameObject dyingBeingPrefab, Transform transformAtDeath)
        {
            this.Initialize(deathCharacteristics, ragdollPrefab, dyingBeingPrefab, transformAtDeath);
            
            if (_death.shouldProduceCorpse() && !_death.shouldBurnCorpse())
            {
                GameObject corpse = _death.shouldProduceRagdollCorpse()
                    ? Instantiate(_ragdollPrefab)
                    : Instantiate(_dyingBeingPrefab);
                if (_death.shouldProduceRagdollCorpse())
                {
                    minimizeMass(corpse);
                }

                if (_death.shouldPropelCorpse())
                {
                    corpse.GetComponentInChildren<Rigidbody>().AddForce((Vector3) _deathCharacteristics["explosion"]);
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