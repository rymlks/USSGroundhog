using EasyState.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace EasyState.Core.Behaviors
{
    internal class DuplicationBehavior : Behavior
    {
        public DuplicationBehavior(Design design) : base(design)
        {
            design.ContextMenuResponse += Design_ContextMenuResponse;
        }

        private void Design_ContextMenuResponse(ContextMenuResponse response, UnityEngine.Vector2 point, string data)
        {
            if (response is ContextMenuResponse.Duplicate)
            {
                bool hasFoundDuplicable = false;
                List<IDuplicable> duplicables = new List<IDuplicable>();
                foreach (var item in Design.GetSelectedModels().ToList())
                {
                    if (item is IDuplicable duplicable)
                    {
                        hasFoundDuplicable = true;

                        var duplicableItem = duplicable;
                        if (duplicableItem != null)
                        {
                            if (duplicableItem.CanDuplicate)
                            {
                                var duplicatedItem = duplicableItem.Duplicate(Design);
                                duplicables.Add(duplicatedItem);
                               
                            }
                        }
                    }
                }
                if (hasFoundDuplicable)
                {
                    Design.UnselectAll();
                    foreach (var duplicated in duplicables)
                    {
                        var selectable = (SelectableModel)duplicated;
                        if (selectable != null)
                        {
                            Design.SelectModel(selectable, false);
                        }
                    }
                    Design.OnShowToast("Duplicated", 1500);
                }
            }
        }

        public override void Dispose()
        {
            Design.ContextMenuResponse -= Design_ContextMenuResponse;
        }
    }
}
