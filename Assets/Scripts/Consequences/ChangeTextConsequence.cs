#nullable enable
using StaticUtils;
using TMPro;
using Triggers;
using UnityEngine;

namespace Consequences
{
    public class ChangeTextConsequence : AbstractConsequence
    {
        public TMP_Text[] textObjects;
        public string toChangeTo;
        public Color newColor = UnityUtil.GetNullColor();

        public override void Execute(TriggerData? data)
        {
            foreach (var text in textObjects)
            {


                text.text = toChangeTo;
                if (newColor != UnityUtil.GetNullColor())
                {
                    text.color = newColor;
                }
            }
        }
    }
}