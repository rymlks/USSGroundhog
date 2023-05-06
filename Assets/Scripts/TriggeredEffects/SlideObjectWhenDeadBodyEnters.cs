using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class SlideObjectWhenDeadBodyEnters: MonoBehaviour
{
    public bool destroy = false;
    public bool reverseOnExit = false;
    public SlideToDestination slideMe;

    public string CustomTag = "Corpse";
    public string RequireItem = "";
    private KeyStatusUIController keyUI;

    private AudioSource _audioSource;

    void Start()
    {
        _audioSource = slideMe.gameObject.GetComponent<AudioSource>();
        if (keyUI == null)
        {
            keyUI = FindObjectOfType<KeyStatusUIController>();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (CustomTag == "" || other.CompareTag(CustomTag))
        {
            if (RequireItem != "" && !GameManager.instance.itemIsPossessed(RequireItem))
            {
                this.keyUI.ShowNextFrame();
                return;
            }

            playSoundIfSourcePresent();
            slideMe.start = true;
            slideMe.forward = true;
            if (destroy)
            {
                Destroy(gameObject);
            }
        }
    }

    private void playSoundIfSourcePresent()
    {
        if (_audioSource != null && !_audioSource.isPlaying)
        {
            _audioSource.Play();
        }
    }

    public void OnTriggerStay(Collider other)
    {
        OnTriggerEnter(other);
    }

    public void OnTriggerExit(Collider other)
    {
        if ((CustomTag == "" || other.CompareTag(CustomTag)) && reverseOnExit)
        {
            slideMe.start = true;
            slideMe.forward = false;
        }
    }
}
