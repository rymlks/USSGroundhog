using EasyState.Core.Models;
using EasyState.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace EasyState.Editor.Renderers.DetailRenderers
{
    internal static class PopupFormatters
    {
        internal static string FormatDestName(Node x, List<Node> destNodeOptions)
        {
            var options = destNodeOptions.Where(y => y.Name == x.Name).ToList();
            if (options.Count > 1)
            {
                return $"{x.Name}({x.Position.x},{x.Position.y})";
            }
            return x.Name;
        }
        internal static string FormatFunctionName(FunctionModel x, List<FunctionModel> functionChoices)
        {
            return x.Name;
            //var actions = functionChoices.Where(y => y.Name == x.Name).ToList();
            //if (actions.Count > 1)
            //{
            //    return x.FullName;
            //}
            //else
            //{
            //    return x.Name;
            //}
        }
    }
}
