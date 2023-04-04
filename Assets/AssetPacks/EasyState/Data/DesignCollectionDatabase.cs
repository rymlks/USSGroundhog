using EasyState.DataModels;

namespace EasyState.Data
{
    public class DesignCollectionDatabase : DatabaseFile<DesignDatabaseData>
    {
        public DesignCollectionDatabase() : base("designs.json")
        {
        }
    }
}
