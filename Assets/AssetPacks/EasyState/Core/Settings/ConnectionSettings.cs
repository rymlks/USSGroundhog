using EasyState.Core.Utility;
using System;
using UnityEngine;

namespace EasyState.Settings
{
    [Serializable]
    public class ConnectionSettings
    {
        [Tooltip("Width of line connecting nodes together")]
        public float ConnectionLineWidth = 2f;
        [Tooltip("Size of triangle handle in the center of the connection line")]
        public float HandleSize = 7;
        [Tooltip("Space between input connections and center line of node")]
        public float InputOutputOffset = 10f;
        [Tooltip("Space between output connections and center line of node")]
        public float FallbackConnectionOffset = 10f;
        [Tooltip("Default connection line color")]
        public Color LineColor = new Color(0.1454884f, 0.272f, 0.151814f);
        [Tooltip("Default selected connection line color")]
        public Color SelectedLineColor = EditorColors.Green_Focus;
        [Tooltip("Default fallback connection line color")]
        public Color FallbackLineColor = new Color(0.489f,0, 0.1778183f);
        [Tooltip("Default selected fallback connection line color")]
        public Color SelectedFallbackLineColor = EditorColors.Pink_Focus;
        [Tooltip("Should every new connection have an always true condition added to it?")]
        public bool DefaultToAlwaysTrueCondition= true;
    }
}