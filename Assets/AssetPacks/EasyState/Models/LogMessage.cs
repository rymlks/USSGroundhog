using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyState.Models
{
    [EasyStateScript(Id)]
    public class LogMessage : Action<DataTypeBase, string>
    {
        public const string Id = "easy-state-logmessage-action";
        public LogMessage(string value) : base(value)
        {
        }
        public override void Act(DataTypeBase data, string parameter)=> UnityEngine.Debug.Log(parameter);
    }
}
