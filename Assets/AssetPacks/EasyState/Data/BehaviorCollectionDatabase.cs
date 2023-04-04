using EasyState.DataModels;

namespace EasyState.Data
{
    public class BehaviorCollectionDatabase : DatabaseFile<BehaviorCollectionData>
    {
        public BehaviorCollectionDatabase() : base("behaviors.json")
        {
        }
    }
   
}
