#nullable enable
using System.Collections.Generic;
using System.Linq;
using Consequences;
using UnityEngine;

namespace Triggers
{
    public abstract class AbstractTrigger : MonoBehaviour, ITrigger
    {
        public bool destroy = false;
        public GameObject consequenceObject;
        private AudioSource _audioSource;

        protected List<IConsequence> _allConsequences = new List<IConsequence>();

        protected virtual void Start()
        {
            if (this.consequenceObject == null)
            {
                this.consequenceObject = this.gameObject;
            }
            this._allConsequences = consequenceObject.GetComponents<IConsequence>().ToList();
            this._audioSource = GetComponent<AudioSource>();
        }

        private void ExecuteAllConsequences()
        {
            foreach (IConsequence consequence in this._allConsequences)
            {
                consequence.execute(null);
            }
            playSoundIfSourcePresent();
        }
    
        private void ExecuteAllConsequences(TriggerData data)
        {
            foreach (IConsequence consequence in this._allConsequences)
            {
                consequence.execute(data);
            }
            playSoundIfSourcePresent();
        }

        public virtual void Engage()
        {
            this.ExecuteAllConsequences();
        }

        public void Engage(TriggerData data)
        {
            this.ExecuteAllConsequences(data);
        }

        private void playSoundIfSourcePresent()
        {
            if (_audioSource != null && !_audioSource.isPlaying)
            {
                _audioSource.Play();
            }
        }
    }
}
