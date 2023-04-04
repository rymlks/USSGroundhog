using EasyState.DataModels;
using System.Collections.Generic;

namespace EasyState.Data
{
    public class DesignCollectionFullDatabase
    {
        public List<DesignData> Load()
        {
            var designDataCollection = new DesignCollectionDatabase().Load();

            var designData = new List<DesignData>();
            foreach (var item in designDataCollection.Designs)
            {
                var design = new DesignDatabase(item.Id).Load();
                designData.Add(design);
            }
            return designData;
        }


    }
}
