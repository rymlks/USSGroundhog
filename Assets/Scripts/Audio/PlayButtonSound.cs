using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public AudioClip mouseEnter, mouseExit, confirmClick;
    public AudioSource audioSource;

    public void OnPointerEnter(PointerEventData data) {
        ChangeClip(mouseEnter);
        PlayClip();
    }
    
    public void OnPointerExit(PointerEventData data) {
        ChangeClip(mouseExit);
        PlayClip();
    }
    
    public void OnPointerClick(PointerEventData data) {
        ChangeClip(confirmClick);
        PlayClip();
    }
    
    void ChangeClip(AudioClip clip) => audioSource.clip = clip;
    void PlayClip() => audioSource.Play();
}
