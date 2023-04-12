#nullable enable
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Triggers
{
    public abstract class AbstractTrigger : MonoBehaviour, ITrigger
    {
        public bool destroy = false;
        private List<IConsequence> _allConsequences = new List<IConsequence>();
        private AudioSource _audioSource;
    
        protected virtual void Start()
        {
            this._allConsequences = this.GetComponents<IConsequence>().ToList();
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
