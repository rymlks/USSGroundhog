using EasyState.DataModels;
using System.Collections.Generic;

namespace EasyState.Models
{
    public interface IFunctionModelSet
    {
        List<FunctionModel> GetFunctionModels();
    }
}