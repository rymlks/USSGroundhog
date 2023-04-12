#nullable enable
using Triggers;

namespace Consequences
{
    public class RotateConsequence : DoASpinny, IConsequence
    {

        private float _speed;

        void Start()
        {
            _speed = speed;
            speed = 0;
        }

        public void execute(TriggerData? data)
        {
            speed = _speed;
        }
    }
}
