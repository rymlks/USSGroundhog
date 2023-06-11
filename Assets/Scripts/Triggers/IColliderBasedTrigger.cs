namespace Triggers
{
    public interface IColliderBasedTrigger
    {
        bool RespondsToTriggers();

        bool RespondsToColliders();
    }
}