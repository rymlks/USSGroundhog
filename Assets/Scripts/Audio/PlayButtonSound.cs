using UnityEngine;
using UnityEngine.EventSystems;

namespace Audio
{
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
    
        private void ChangeClip(AudioClip clip) => audioSource.clip = clip;
        private void PlayClip() => audioSource.Play();
    }
}
