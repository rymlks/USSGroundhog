using TMPro;
using UnityEngine;

namespace UI
{
    public class TextUIController : MonoBehaviour, IUIController
    {
        public TextMeshProUGUI textMesh;
        protected float lastAffectedTime;
        public Color alertColor = Color.yellow;
        public float secondsMessagePersists = 1f;

        protected virtual void Start()
        {
            if (textMesh == null)
            {
                this.textMesh = this.GetComponent<TextMeshProUGUI>();
            }
        }
        
        public virtual void ShowNextFrame()
        {   
            this.lastAffectedTime = Time.time;
        }
        
        protected virtual void LateUpdate()
        {
            if (this.shouldShowNow())
            {
                this.textMesh.color = alertColor;
            }
            else
            {
                if(this.textMesh)
                    this.textMesh.color = Color.clear;
            }
        }
        
        protected virtual bool shouldShowNow()
        {
            float timeSinceLastAffected = Time.time - this.lastAffectedTime;
            return timeSinceLastAffected >= 0 && timeSinceLastAffected < secondsMessagePersists;
        }
    }
}