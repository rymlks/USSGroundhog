using System;

namespace EasyState.Models
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class EasyStateIgnoreScriptAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class EasyStateScriptAttribute : Attribute
    {
        public string ScriptID { get; }

        public EasyStateScriptAttribute(string scriptID)
        {
            ScriptID = scriptID;
        }
    }
}