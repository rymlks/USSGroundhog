#nullable enable
using System.Collections.Generic;
using System.Linq;
using Consequences;
using O3DWB;
using UnityEngine;

namespace Triggers
{
    public abstract class AbstractTrigger : MonoBehaviour, ITrigger
    {
        public bool destroy = false;
        public GameObject consequenceObject;
        public bool findConsequencesInChildren = false;
        public Behaviour[] requireComponentsEnabled;
        public Behaviour[] requireComponentsDisabled;
        private List<IConsequence> _allConsequences = new List<IConsequence>();
        private AudioSource _audioSource;
    
        protected virtual void Start()
        {
            if (this.consequenceObject == null)
            {
                this.consequenceObject = this.gameObject;
            }
            this._allConsequences = (this.findConsequencesInChildren ? consequenceObject.GetComponentsInChildren<IConsequence>() : consequenceObject.GetComponents<IConsequence>()).ToList();
            this._audioSource = GetComponent<AudioSource>();
        }

        private void ExecuteAllConsequences()
        {
            foreach (IConsequence consequence in this._allConsequences)
            {
                consequence.Execute(null);
            }
            playSoundIfSourcePresent();
        }
    
        private void ExecuteAllConsequences(TriggerData data)
        {
            foreach (IConsequence consequence in this._allConsequences)
            {
                consequence.Execute(data);
            }
            playSoundIfSourcePresent();
        }
        
        private void CancelAllCancelableConsequences(TriggerData data)
        {
            foreach (IConsequence consequence in this._allConsequences)
            {
                ICancelableConsequence cancelable = (ICancelableConsequence) consequence;
                if (cancelable != null)
                {
                    cancelable.Cancel(data);
                }
            }
            playSoundIfSourcePresent();
        }

        public virtual void Engage()
        {
            if (meetsRequirements())
            {
                this.ExecuteAllConsequences();
                if (destroy)
                {
                    Destroy(gameObject);
                }
            }
        }
        
        public virtual void Disengage(TriggerData data)
        {
            this.CancelAllCancelableConsequences(data);
        }

        public virtual void Engage(TriggerData data)
        {
            if (meetsRequirements())
            {
                this.ExecuteAllConsequences(data);
                if (destroy)
                {
                    Destroy(gameObject);
                }
            }
        }

        private void playSoundIfSourcePresent()
        {
            if (_audioSource != null && _audioSource.enabled && !_audioSource.isPlaying)
            {
                _audioSource.Play();
            }
        }

        protected virtual bool meetsRequirements()
        {
            foreach (Behaviour compo in requireComponentsEnabled)
            {
                if (!compo.isActiveAndEnabled)
                {
                    return false;
                }
            }
            foreach (Behaviour compo in requireComponentsDisabled)
            {
                if (compo.isActiveAndEnabled)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
