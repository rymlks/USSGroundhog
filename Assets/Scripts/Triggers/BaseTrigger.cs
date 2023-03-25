using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTrigger : MonoBehaviour
{
    public bool destroy = false;
    public GameObject consequenceObject;
    public IConsequence triggeredEffect;
    private AudioSource _audioSource;

    public void Start()
    {
        
        triggeredEffect = consequenceObject.GetComponentInChildren<IConsequence>();
    }

    public void Trigger()
    {
        Debug.Log("triggering");
        playSoundIfSourcePresent();
        triggeredEffect.execute();
    }

    public void UnTrigger()
    {
        // TODO
    }

    private void playSoundIfSourcePresent()
    {
        if (_audioSource != null && !_audioSource.isPlaying)
        {
            _audioSource.Play();
        }
    }
}
