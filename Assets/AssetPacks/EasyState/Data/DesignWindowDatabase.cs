using EasyState.DataModels;

namespace EasyState.Data
{
    public class DesignWindowDatabase : DatabaseFile<DesignerWindowState>
    {

        public DesignWindowDatabase() : base("designer.json")
        {
        }
        
    }
}
