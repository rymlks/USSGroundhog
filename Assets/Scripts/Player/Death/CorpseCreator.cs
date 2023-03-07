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
        private static readonly int IsFallDead = Animator.StringToHash("IsFallDead");
        private static readonly int IsElectrocuted = Animator.StringToHash("IsElectrocuted");

        protected void Initialize(Dictionary<string, object> deathCharacteristics, GameObject ragdollPrefab,
            GameObject dyingBeingPrefab, Transform transformAtDeath)
        {
            this._deathCharacteristics = deathCharacteristics;
            this._ragdollPrefab = ragdollPrefab;
            this._dyingBeingPrefab = dyingBeingPrefab;
            this._transformAtDeath = transformAtDeath;
        }

        protected bool shouldProduceRagdollCorpse()
        {
            return _deathCharacteristics.Keys.Contains("ragdoll");
        }

        protected bool shouldProduceNormalCorpse()
        {
            return !(shouldBurnCorpse() || shouldProduceRagdollCorpse() ||
                     shouldProduceRigidCorpse());
        }

        protected bool shouldProduceRigidCorpse()
        {
            return _deathCharacteristics.Keys.Contains("freezing");
        }

        protected bool shouldBurnCorpse()
        {
            return _deathCharacteristics.Keys.Contains("burning");
        }

        protected bool shouldPropelCorpse()
        {
            return _deathCharacteristics.Keys.Contains("explosion");
        }

        protected bool shouldProduceCorpse()
        {
            return shouldProduceRagdollCorpse() || shouldProduceRigidCorpse() || shouldProduceNormalCorpse();
        }

        public void CreateAndAnimateCorpse(Dictionary<string, object> deathCharacteristics, GameObject ragdollPrefab,
            GameObject dyingBeingPrefab, Transform transformAtDeath)
        {
            this.Initialize(deathCharacteristics, ragdollPrefab, dyingBeingPrefab, transformAtDeath);
            
            if (shouldProduceCorpse() && !shouldBurnCorpse())
            {
                GameObject corpse = shouldProduceRagdollCorpse()
                    ? Instantiate(_ragdollPrefab)
                    : Instantiate(_dyingBeingPrefab);
                if (shouldProduceRagdollCorpse())
                {
                    minimizeMass(corpse);
                }

                if (shouldPropelCorpse())
                {
                    corpse.GetComponentInChildren<Rigidbody>().AddForce((Vector3) _deathCharacteristics["explosion"]);
                }

                corpse.transform.position = _transformAtDeath.position;
                corpse.transform.rotation = _transformAtDeath.rotation;
                corpse.tag = "Corpse";

                AnimateCorpse(corpse);
            }
        }

        private void AnimateCorpse(GameObject corpse)
        {
            if (shouldProduceNormalCorpse())
            {
                corpse.GetComponentInChildren<Animator>().SetBool(IsFallDead, true);
            }
            else if (_deathCharacteristics.Keys.Contains("electrocution"))
            {
                corpse.GetComponentInChildren<Animator>().SetBool(IsElectrocuted, true);
            }
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