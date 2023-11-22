using Audio;
using Triggers;
using Unity.VisualScripting;
using UnityEngine;

namespace Consequences.TagAffecting
{
    public class DestroyGameObjectsByTagConsequence : TagBasedConsequence
    {
        public GameObject effectToPlayAtDestruction;
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
            if (effectToPlayAtDestruction != null)
            {
                var instantiatedEffect = GameObject.Instantiate(this.effectToPlayAtDestruction, toDestroy.transform.position,
                    toDestroy.transform.rotation, null);
                instantiatedEffect.GetComponent<ParticleSystem>().Play();
            }

            if (audioToPlayAtDestruction != null)
            {
                GameObject audioObject = new GameObject();
                audioObject.transform.position = toDestroy.transform.position;
                audioObject.AddComponent<AudioSource>().PlayOneShot(audioToPlayAtDestruction);
            }

            Destroy(toDestroy);
        }
    }
}