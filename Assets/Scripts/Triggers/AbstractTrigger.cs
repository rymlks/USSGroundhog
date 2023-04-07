using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AbstractTrigger : MonoBehaviour, ITrigger
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
            consequence.execute();
        }
        playSoundIfSourcePresent();
    }

    public virtual void Engage()
    {
        this.ExecuteAllConsequences();
    }
    
    private void playSoundIfSourcePresent()
    {
        if (_audioSource != null && !_audioSource.isPlaying)
        {
            _audioSource.Play();
        }
    }
}
