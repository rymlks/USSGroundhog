using TMPro;
using UnityEngine;

namespace UI
{
    public class TextUIController : AbstractUIController
    {
        public TextMeshProUGUI textMesh;
        public Color alertColor = Color.yellow;

        protected virtual void Start()
        {
            if (textMesh == null)
            {
                this.textMesh = this.GetComponent<TextMeshProUGUI>();
            }
        }

        protected override void DisableUI()
        {
            if (this.textMesh)
                this.textMesh.color = Color.clear;
        }

        protected override void EnableUI()
        {
            this.textMesh.color = alertColor;
        }
    }
}