using EasyState.DataModels;
using System.Linq;

namespace EasyState.Data
{
    public class DesignDatabase : DatabaseFile<DesignData>
    {
        private readonly string _designID;

        public DesignDatabase(string designID) : base($"design-{designID}.json")
        {
            _designID = designID;
        }

        protected override void OnFileDeleted()
        {
            //remove design from design collection
            var db = new DesignCollectionDatabase();
            var designCollection = db.Load();
            var designDeleted = designCollection.Designs.FirstOrDefault(x => x.Id == _designID);
            if (designDeleted != null)
            {
                designCollection.Designs.Remove(designDeleted);
                db.Save(designCollection);
                LogFileChange("Removed a design from the design collection");
            }
            //remove associated behavior if one exists
            var behaviorDb = new BehaviorCollectionDatabase();
            var behaviorCollection = behaviorDb.Load();
            var associatedBehavior = behaviorCollection.Behaviors.FirstOrDefault(x=> x.DesignId == _designID);
            if(associatedBehavior != null)
            {
                behaviorCollection.Behaviors.Remove(associatedBehavior);
                behaviorDb.Save(behaviorCollection);
                LogFileChange("Removed an associated behavior, because a design was deleted.");
            }
            //remove design from designer if open
            var designerStateDb = new DesignWindowDatabase();
            var windowState = designerStateDb.Load();
            var openDesign = windowState.OpenDesigns.FirstOrDefault(x=> x.Id == _designID);
            if(openDesign != null)
            {                
                windowState.OpenDesigns.Remove(openDesign);
                designerStateDb.Save(windowState);
                LogFileChange("Closed and deleted tab data because its source design was deleted.");
            }

        }
        protected override void OnFileChanged(DesignData data, bool isUpdate)
        {
            //add or update design short in design collection
            var db = new DesignCollectionDatabase();
            var designCollection = db.Load();
            var dataTypes = new DataTypeDatabase().Load();
            var designDataType = dataTypes.DataTypes.First(x => x.Id == data.DataTypeID);
            var updatedDesignShort = new DesignDataShort(data, designDataType);
            if (isUpdate)
            {
                var designToUpdateIndex = designCollection.Designs.FindIndex(x => x.Id == data.Id);
                designCollection.Designs[designToUpdateIndex] = updatedDesignShort;
            }
            else
            {
                designCollection.Designs.Add(updatedDesignShort);
            }
            db.Save(designCollection);
        }
       
    }
}