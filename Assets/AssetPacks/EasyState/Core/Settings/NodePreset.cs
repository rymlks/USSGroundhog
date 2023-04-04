using EasyState.DataModels;
using System;
using UnityEngine;

namespace EasyState.Settings
{
    [Serializable]
    public class NodePreset
    {
        public NodeActionExecutionType ActionExecutionType;
        public NodeConditionType ConditionType;
        public NodeCycleType CycleType;
        public Color NodeColor;
        public string PresetName;
        public Color SelectedColor;
    }
}