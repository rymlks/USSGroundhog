#nullable enable
using Triggers;
using UnityEngine;

namespace Consequences
{
    public abstract class DetachableConsequence : AbstractConsequence
    {
        [Tooltip(
            "If true, the particle systems will be detached from their parent GameObjects before playing.  Useful if the parent will cease to exist before the system is finished.")]
        public bool detachBeforePlaying = false;
    }
}