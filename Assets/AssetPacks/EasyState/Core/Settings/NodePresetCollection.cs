using EasyState.Core.Models;
using EasyState.Core.Utility;
using EasyState.DataModels;
using System;
using System.Linq;
using UnityEngine;

namespace EasyState.Settings
{
    [Serializable]
    public class NodePresetCollection
    {
        public const string CONDITIONAL_EXECUTOR_NODE = "Executor Node";
        public const string ENTRY_NODE = "Entry Node";
        public const string REPEATER_NODE = "Repeater Node";
        public const string STATE_NODE = "State Node";
        public const string UTILITY_NODE = "Utility Node";
        public NodePreset[] NodePresets;
        public Color JumpNodeColor = EditorColors.DarkPink;
        public Color SelectedJumpNodeColor = EditorColors.DarkPink_Focus;
        public Node BuildEntryNode()
        {
            var node = new Node(isEntryNode: true, position: new Vector2(550, 375))
            {
                ActionExecutionType = NodeActionExecutionType.Default,
                ConditionType = NodeConditionType.Default,
                CycleType = NodeCycleType.PassThrough,
                Name = ENTRY_NODE,
                SelectedNodeColor = EditorColors.Green_Focus,
                NodeColor = EditorColors.Green
            };

            return node;
        }

        public Node BuildNodeFromPreset(string presetName)
        {
            var preset = NodePresets.FirstOrDefault(x => x.PresetName == presetName);
            if (preset is null)
            {
                return null;
            }

            return new Node()
            {
                ActionExecutionType = preset.ActionExecutionType,
                ConditionType = preset.ConditionType,
                CycleType = preset.CycleType,
                Name = preset.PresetName,
                SelectedNodeColor = preset.SelectedColor,
                NodeColor = preset.NodeColor
            };
        }

        public void Reset()
        {
            NodePresets = new NodePreset[]
            {
            new NodePreset
            {
                PresetName = STATE_NODE,
                ActionExecutionType = NodeActionExecutionType.Default,
                ConditionType = NodeConditionType.Default,
                CycleType = NodeCycleType.PassThrough,
                NodeColor = EditorColors.Blue,
                SelectedColor = EditorColors.Blue_Focus
            },
             new NodePreset
            {
                PresetName = REPEATER_NODE,
                ActionExecutionType = NodeActionExecutionType.Default,
                ConditionType = NodeConditionType.Repeat,
                CycleType = NodeCycleType.YieldCycle,
                NodeColor = EditorColors.Orange,
                SelectedColor = EditorColors.Orange_Focus
            },
            new NodePreset
            {
                PresetName = CONDITIONAL_EXECUTOR_NODE,
                ActionExecutionType = NodeActionExecutionType.ConditionalExecution,
                ConditionType = NodeConditionType.Default,
                CycleType = NodeCycleType.PassThrough,
                NodeColor = EditorColors.Purple,
                SelectedColor = EditorColors.Purple_Focus
            },
            new NodePreset
            {
                PresetName = UTILITY_NODE,
                ActionExecutionType = NodeActionExecutionType.Default,
                ConditionType = NodeConditionType.Utility,
                CycleType = NodeCycleType.PassThrough,
                NodeColor = EditorColors.Yellow,
                SelectedColor = EditorColors.Yellow_Focus
            },
            };
        }
    }
}