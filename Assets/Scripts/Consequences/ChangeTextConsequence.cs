#nullable enable
using StaticUtils;
using TMPro;
using Triggers;
using UnityEngine;

namespace Consequences
{
    public class ChangeTextConsequence : AbstractConsequence
    {
        public TMP_Text textObject;
        public string toChangeTo;
        public Color newColor = UnityUtil.GetNullColor();

        public override void Execute(TriggerData? data)
        {
            textObject.text = toChangeTo;
            if (newColor != UnityUtil.GetNullColor())
            {
                textObject.color = newColor;
            }
        }
    }
}