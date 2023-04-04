using EasyState.DataModels;
using EasyState.Core.Models;
using EasyState.Data;
using EasyState.Settings;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace EasyState.Editor.Renderers.DetailRenderers
{
    class JumperNodeDetailRenderer : DetailRendererBase<Node>
    {
        private List<DesignDataShort> _designOptions;
        private VisualElement _inputElement;
        private TextField _summaryInput;
        private DesignDataShort nullDesign = new DesignDataShort() { Id = "-1", Name = "Select Dest" };
        public JumperNodeDetailRenderer(VisualElement container, Node model, Design design) : base(container, model, design)
        {
            _inputElement = container.Q<VisualElement>("dest-input");
            var designDatabase = new DesignCollectionDatabase().Load();
            var dataTypeDatabase = new DataTypeDatabase().Load();
            var dataType = dataTypeDatabase.DataTypes.First(x => x.Id == design.DataTypeID);
            var behaviors = new BehaviorCollectionDatabase().Load();
            _designOptions = designDatabase.Designs.Where(x => (x.DataTypeID == design.DataTypeID || x.DataTypeID == dataType.ParentDataTypeID) && x.Id != design.Id).ToList();
            _designOptions = _designOptions.Where(x => behaviors.Behaviors.Any(y => y.DesignId == x.Id)).ToList();
            _designOptions.Insert(0,nullDesign);
            _summaryInput = container.Q<TextField>("summary-input");
            _summaryInput.SetValueWithoutNotify(Model.NodeSummary);
            _summaryInput.RegisterValueChangedCallback(x =>
            {
                Model.NodeSummary = x.newValue;
                Model.Refresh();
            });
            BuildPopups();
        }

        private void BuildPopups()
        {
            _inputElement.Clear();
            var popup = new PopupField<DesignDataShort>("Destination", _designOptions, nullDesign, x => x.Name, x => x.Name);
            if (Model.JumpDesign != null)
            {
                var design = _designOptions.FirstOrDefault(x => x.Id == Model.JumpDesign.Id);
                popup.SetValueWithoutNotify(design);
            }
            popup.RegisterValueChangedCallback(x =>
            {
                if (x.newValue == nullDesign)
                {
                    Model.JumpDesign = null;
                    Model.JumpNode = null;
                }
                else
                {
                    Model.JumpDesign = x.newValue;

                    var entryNode = Model.JumpDesign.Nodes.First(y => y.Name == NodePresetCollection.ENTRY_NODE);

                    Model.JumpNode = entryNode;
                }
                BuildPopups();
            });

            _inputElement.Add(popup);
            if (Model.JumpDesign != null)
            {

                var nodes = Model.JumpDesign.Nodes;

                var nodePopup = new PopupField<NodeDataShort>("Dest Node", nodes, 0, x => x.Name, x => x.Name);
                nodePopup.SetValueWithoutNotify(Model.JumpNode);

                nodePopup.RegisterValueChangedCallback(x =>
                {
                    Model.JumpNode = x.newValue;
                });

                _inputElement.Add(nodePopup);


                var row = new VisualElement();
                row.style.flexDirection = FlexDirection.RowReverse;
                row.style.marginTop = 10;
                var jumpBtn = new Label("Go to Destination Design");
                jumpBtn.AddToClassList("btn");
                jumpBtn.AddManipulator(new Clickable(() => Design.OnJumpToDesign(Model.JumpDesign.Id, Model.JumpNode.Id)));
                row.Add(jumpBtn);
                _inputElement.Add(row);
            }
        }

        protected override string Title => "Edit Jump Node";

        protected override void AddPropertyFields()
        {
            PropertyCollection.AddTextProperty(x => x.Name, "name-input");
        }
    }
}
