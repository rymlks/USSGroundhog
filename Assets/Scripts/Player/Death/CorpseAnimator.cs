using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Player.Death
{
    public class CorpseAnimator
    {
        private static readonly int IsFallDead = Animator.StringToHash("IsFallDead");
        private static readonly int IsElectrocuted = Animator.StringToHash("IsElectrocuted");

        public void AnimateCorpse(GameObject corpse, Dictionary<string, object> deathCharacteristics)
        {
            DeathCharacteristicsProcessor death = new DeathCharacteristicsProcessor(deathCharacteristics);
            if (death.shouldProduceElectrocutedCorpse())
            {
                corpse.GetComponentInChildren<Animator>().SetBool(IsElectrocuted, true);
            }else if (death.shouldProduceNormalCorpse())
            {
                corpse.GetComponentInChildren<Animator>().SetBool(IsFallDead, true);
            }
        }
    }
}