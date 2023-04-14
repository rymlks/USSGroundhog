namespace Triggers
{
    public interface ITrigger
    {
        void Engage();

        void Engage(TriggerData data);
    }
}
