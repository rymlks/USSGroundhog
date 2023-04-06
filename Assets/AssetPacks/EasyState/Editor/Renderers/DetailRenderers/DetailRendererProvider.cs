using EasyState.Core.Models;
using EasyState.Editor.Templates;
using System;
using System.Collections.Generic;

namespace EasyState.Editor.Renderers.DetailRenderers
{
    internal class DetailRendererProvider
    {
        private Dictionary<Type, IDetailRenderer> _rendererRegistry = new Dictionary<Type, IDetailRenderer>();

        public DetailRendererProvider()
        {
        }

        public IDetailRenderer GetDetailRenderer<TModel>(TModel model, Design design)
        {
            string typeName = model.GetType().Name;
            switch (typeName)
            {
                case nameof(Node):
                    var node = model as Node;
                    if (node.IsJumpNode)
                    {
                        return new JumperNodeDetailRenderer(TemplateFactory.CreateDetailTemplate("JumperNode"), node, design);
                    }
                    else
                    {
                        return new NodeDetailRenderer(TemplateFactory.CreateDetailTemplate("Node"), model as Node, design);
                    }

                case nameof(Note):
                    return new NoteDetailsRenderer(TemplateFactory.CreateDetailTemplate("Note"), model as Note, design);

                case nameof(Connection):
                    return new ConnectionDetailRenderer(TemplateFactory.CreateDetailTemplate("Connection"), model as Connection, design);

                default:
                    throw new NotImplementedException($"{typeof(TModel).Name} has no associated IDetailRenderer registered");
            }
        }
    }
}