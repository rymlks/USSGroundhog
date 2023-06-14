#nullable enable
using Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace Consequences.UI
{
    public class BlinkButtonColorConsequence : AbstractInterruptibleConsequence
    {
        public Button controlledButton;
        public float blinkTime = 0f;
        protected Color _originalHighlightedColor;

        void Update()
        {
            if (this.started && blinkTime > 0f)
            {
                ColorBlock newColors = this.controlledButton.colors;
                newColors.highlightedColor = Color.Lerp(this._originalHighlightedColor, newColors.selectedColor,
                    getTimeScalar());
                this.controlledButton.colors = newColors;
            }
        }

        protected float getTimeScalar()
        {
            if (float.IsPositiveInfinity(_startTime))
            {
                return 0;
            }
            else
            {
                int blinks = (int)(timeSinceStarted() / blinkTime);
                float remainingFraction = (timeSinceStarted() % blinkTime);
                if(blinks % 2 == 0)
                {
                    return remainingFraction / blinkTime;
                }
                else
                {
                    return (blinkTime - remainingFraction) / blinkTime;
                }
            }
        }

        protected float timeSinceStarted()
        {
            return Time.time - _startTime;
        }

        public override void Execute(TriggerData? data)
        {
            base.Execute(data);
            this._originalHighlightedColor = controlledButton.colors.highlightedColor;
        }
    }
}