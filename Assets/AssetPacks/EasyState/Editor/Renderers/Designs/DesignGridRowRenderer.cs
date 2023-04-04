using EasyState.DataModels;
using EasyState.Editor.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UIElements;

namespace EasyState.Editor.Renderers.Designs
{
    public class DesignGridRowRenderer
    {
        public readonly DesignDataShort Design;
        public readonly VisualElement Element;
        public DesignGridRowRenderer(DesignDataShort design,VisualElement parent, Action<DesignGridRowRenderer> onDelete, Action<DesignGridRowRenderer> onLoad, bool hasBehavior)
        {
            Design = design;
            Element = TemplateFactory.CreateDesignGridRowTemplate();
          
            var designName = Element.Q<Label>("design-name");
            designName.text = Design.Name;
            var dataTypeName = Element.Q<Label>("data-type-name");
            dataTypeName.text = Design.DataTypeName;
            var behaviorCol = Element.Q<Label>("has-behavior");
            behaviorCol.text = hasBehavior ? "Yes" : "No";
            var nodeCount = Element.Q<Label>("node-count");
            nodeCount.text = Design.Nodes?.Count.ToString();
            var connCount = Element.Q<Label>("connection-count");
            connCount.text = Design.ConnectionCount.ToString();

            var loadBtn = Element.Q<Label>("load-design");
            loadBtn.AddManipulator(new Clickable(() => onLoad.Invoke(this)));
            
            var deleteBtn = Element.Q<Label>("delete-design");
            deleteBtn.AddManipulator(new Clickable(() => onDelete.Invoke(this)));

            parent.Add(Element);
        }
    }
}
