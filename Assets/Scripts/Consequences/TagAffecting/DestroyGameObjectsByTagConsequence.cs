using Triggers;
using UnityEngine;

namespace Consequences.TagAffecting
{
    public class DestroyGameObjectsByTagConsequence : TagBasedConsequence
    {
        public ParticleSystem effectToPlayAtDestruction;
        public AudioClip audioToPlayAtDestruction;

        public override void Execute(TriggerData? data)
        {
            if (TagToDetect != "")
            {
                foreach (GameObject destroying in GameObject.FindGameObjectsWithTag(TagToDetect))
                {
                    doDestroy(destroying);
                }
            }

            foreach (var additionalTag in AdditionalTagsToDetect)
            {
                foreach (GameObject destroying in GameObject.FindGameObjectsWithTag(additionalTag))
                {
                    doDestroy(destroying);
                }
            }
        }

        protected void doDestroy(GameObject toDestroy)
        {
            Destroy(toDestroy);
        }
    }
}