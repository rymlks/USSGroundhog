using EasyState.Core.Models;
using EasyState.Core.Validators;
using EasyState.Data;
using EasyState.DataModels;
using Newtonsoft.Json;
using System.Linq;
using System.Text;
using UnityEditor;

namespace EasyState.Editor.Utility
{
    internal static class DesignExetensions
    {
        private static readonly string _invalidDesignExisingDesign = "This design is in an invalid state. If saved associated behavior will be deleted.";
        private static readonly string _invalidAssociatedDesignExisingDesign = "This design is associated to an invalid state. If saved associated behavior will be deleted.";
        internal static void SaveChanges(this Design design)
        {
            var db = new DesignDatabase(design.Id);
            var behaviorDb = new BehaviorCollectionDatabase();
            var behaviorCollection = behaviorDb.Load();
            var existingBehavior = behaviorCollection.Behaviors.FirstOrDefault(x => x.DesignId == design.Id);
            var designData = design.Serialize();
            var validationResult = DesignValidator.ValidateDesign(designData);
            bool saveAuthorized = true;
            bool foundErrors = !validationResult;
            if (!validationResult && existingBehavior != null)
            {
                //immeadiate design has errors
                saveAuthorized = EditorUtility.DisplayDialog("Invalid Design", _invalidDesignExisingDesign, "Ok", "Cancel");
                foundErrors = true;
            }
            else if (existingBehavior != null)
            {

                foreach (var designID in validationResult.AdditionalDesignsToValidate)
                {
                    var additionalData = new DesignDatabase(designID).Load();
                    var jumpDesignValidationResult = DesignValidator.ValidateDesign(designData);
                    if (!jumpDesignValidationResult)
                    {
                        //associated design has errors
                        foundErrors = true;
                        break;
                    }
                }
                if (foundErrors)
                {
                    saveAuthorized = EditorUtility.DisplayDialog("Invalid Associated Design", _invalidAssociatedDesignExisingDesign, "Ok", "Cancel");
                }

            }
            //save design changes
            if (saveAuthorized)
            {
                db.Save(designData);
                design.OnShowToast("Changes saved", 1500);
            }
            //design was invalid remove existing behavior
            if (foundErrors && saveAuthorized && existingBehavior != null)
            {
                behaviorCollection.Behaviors.Remove(existingBehavior);

                behaviorDb.Save(behaviorCollection);
                design.OnShowToast("Behavior deleted", 2500);
            }
            //design was valid, and had no previous behavior so create one
            else if (saveAuthorized && existingBehavior == null && !foundErrors)
            {
                behaviorCollection.Behaviors.Add(new DataModels.BehaviorData
                {
                    BehaviorName = design.Name,
                    DataTypeId = design.DataTypeID,
                    DesignId = design.Id,
                    DataTypeFullName = design.DataType.AssemblyQualifiedName
                });
                behaviorDb.Save(behaviorCollection);
                design.OnShowToast("Behavior created", 2500);
            }
            //design was valid and had a previous associated behavior so update it
            else if (saveAuthorized && existingBehavior != null && !foundErrors)
            {

                existingBehavior.BehaviorName = design.Name;
                existingBehavior.DataTypeFullName = design.DataType.AssemblyQualifiedName;

                behaviorDb.Save(behaviorCollection);

                design.OnShowToast("Behavior updated", 2500);
            }

        }
        internal static DesignData RandomizeStateIDs(DesignData design)
        {
            StringBuilder sb = new StringBuilder(JsonConvert.SerializeObject(design));

            foreach (var node in design.Nodes)
            {
                var oldId = node.Id;
                var newId = GUID.Generate().ToString();
                sb.Replace(oldId, newId);
            }
            return JsonConvert.DeserializeObject<DesignData>(sb.ToString());
        }
    }
}
