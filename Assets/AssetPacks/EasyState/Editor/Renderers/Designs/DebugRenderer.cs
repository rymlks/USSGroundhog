using EasyState.Core.Models;
using EasyState.Editor.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;

namespace EasyState.Editor.Renderers.Designs
{
    public class DebugRenderer : ModelRenderer<Design>, IDisposable
    {
        public override VisualElement Element { get; protected set; }
        public const int LOG_MESSAGE_LIMIT = 15;
        private readonly Label _designDebugLabel;
        private readonly Label _logLabel;
        private readonly LinkedList<string> _logMessages = new LinkedList<string>();
        private readonly Label _mousePositionLabel;
        private readonly Label _trackingLabel;

        public DebugRenderer(Design model, VisualElement container) : base(model)
        {            
            Element = TemplateFactory.CreateDebugTemplate();
            container.Add(Element);
            Element.style.bottom = 10;
            Element.style.right = 10;
            _mousePositionLabel = Element.Q<Label>("mouse-pos");
            _designDebugLabel = Element.Q<Label>("design-debug");
            _trackingLabel = Element.Q<Label>("tracking");
            _logLabel = Element.Q<Label>("log");
            Model.MouseMove += Model_MouseMove;
            Model.Changed += Model_Changed;
            Model.ZoomChanged += Model_Changed;
            Model.PanChanged += Model_Changed;
            Model.SelectionChanged += Model_SelectionChanged;
            Model.Message += Model_Message;
            Model_Changed();
        }

        public override void Dispose()
        {
            Model.SelectionChanged -= Model_SelectionChanged;
            Model.MouseMove -= Model_MouseMove;
            Model.Changed -= Model_Changed;
            Model.ZoomChanged -= Model_Changed;
            Model.Message -= Model_Message;
            Model.PanChanged -= Model_Changed;
        }

        private string GetConnectionDebug(SelectableModel model)
        {
            var con = model as Connection;

            return $@"
Selected Connection
Id : {con.Id}
Position : {con.Position},
Auto Position : {con.AutoPosition},
Rect : {con.Rect},
Source Node : {con.SourceNode.Id},
Dest Node : {con.DestNode.Id}
";
        }

        private string GetNodeDebug(SelectableModel model)
        {
            Node node = model as Node;
            return $@"
Selected Node : {node.Name ?? "Not named"}
Id : {node.Id}
Position : {node.Position}
Rect : {node.Rect}
Connections : {node.Connections.Count}
";
        }

        private void Model_Changed()
        {
            IEnumerable<SelectableModel> selectedModels = Model.GetSelectedModels();
            string selectedModelsString = "Selected Item : None";
            if (selectedModels.Any())
            {
                if (selectedModels.Count() > 1)
                {
                    selectedModelsString = "Selected Items : " + string.Join(",", selectedModels.Select(x => x.Id));
                }
                else
                {
                    var firstModel = selectedModels.First();
                    if (firstModel is Node)
                    {
                        selectedModelsString = GetNodeDebug(firstModel);
                    }
                    else if (firstModel is Connection)
                    {
                        selectedModelsString = GetConnectionDebug(firstModel);
                    }
                    else
                    {
                        selectedModelsString = "Selected Item : " + selectedModels.First().Id;
                    }
                }
            }
            _designDebugLabel.text = $@"
Design : {Model.Name}
Id :{Model.Id}
State: {Model.State}
Nodes : {Model.Nodes.Count}
Connections : {Model.Nodes.SelectMany(x => x.Connections).Count()}
Pan : {Model.Pan}
Zoom : {Model.Zoom}
{selectedModelsString}
";
        }

        private void Model_Message(object sender, string log)
        {
            if (_logMessages.Count >= LOG_MESSAGE_LIMIT)
            {
                _logMessages.RemoveFirst();
                _logMessages.AddLast(log);
            }
            else
            {
                _logMessages.AddLast(log);
            }
            _logLabel.text = $"Logs :\n{string.Join("\n", _logMessages)}";
        }

        private void Model_MouseMove(Model arg1, MouseMoveEvent evt)
        {
            _mousePositionLabel.text = $"Mouse Position : {evt.mousePosition}\nRelative Position : {Model.GetRelativePosition(evt.mousePosition)}";
        }

        private void Model_SelectionChanged(SelectableModel obj)
        {
            Model_Changed();
        }
    }
}