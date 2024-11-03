using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TextUIController : AbstractUIController
    {
        public TextMeshProUGUI textMesh;
        public RawImage textDecoration;
        public Color alertColor = Color.yellow;

        protected virtual void Start()
        {
            if (textMesh == null)
            {
                if (!this.textMesh)
                    this.textMesh = this.GetComponent<TextMeshProUGUI>();
                if (!this.textDecoration)
                    this.textDecoration = this.GetComponentInParent<RawImage>();
            }
        }

        protected override void DisableUI()
        {
            if (this.textMesh)
                this.textMesh.color = Color.clear;
            if (this.textDecoration)
                this.textDecoration.color = Color.clear;
        }

        protected override void EnableUI()
        {
            this.textMesh.color = alertColor;
            if (this.textDecoration) ;
            {
                this.textDecoration.color = alertColor;
            }
        }
    }
}