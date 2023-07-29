using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MaskedTextUIController : TextUIController
    {
        public RectMask2D mask;
        protected float maxMaskWidth;
        protected override void Start()
        {
            base.Start();
            if (mask == null)
            {
                this.mask = this.textMesh.transform.parent.GetComponent<RectMask2D>();
                if (mask == null)
                {
                    Debug.Log("Masked text controller cannot find RectMask2D as parent of text!");
                }
                this.maxMaskWidth = this.mask.rectTransform.rect.width;
            }
        }

        protected void MaskText(float widthScalar)
        {
            this.mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, widthScalar * maxMaskWidth);
        }
    }
}