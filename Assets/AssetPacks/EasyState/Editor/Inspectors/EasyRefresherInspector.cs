using UnityEditor;

namespace EasyState.Editor.Inspectors
{
    [CustomEditor(typeof(EasyRefresher))]
    public class EasyRefresherInspector : UnityEditor.Editor
    {
        private bool _hasDrawnSomething;

        private EasyRefresher _refresher;

        public override void OnInspectorGUI()
        {
            _hasDrawnSomething = false;
            DrawLabel(_refresher.BackgroundRefresherCount, "Background Refreshers");
            DrawLabel(_refresher.BackgroundWithEventSyncCount, "Background Refresher(with Event Sync)");
            DrawLabel(_refresher.CustomRateMachineCount, "Custom Rate Refreshers");
            DrawLabel(_refresher.FixedUpdateRefreshCount, "Fixed Update Refreshers");
            DrawLabel(_refresher.UpdateRefresherCount, "Refreshers in Update");
            DrawLabel(_refresher.LateUpdateRefreshCount, "Late Update Refreshers");
            DrawLabel(_refresher.TotalRefreshCount, "Total Machines being Refreshed");
            if (!_hasDrawnSomething)
            {
                EditorGUILayout.LabelField("There are no machines being updated currently.");
            }
        }

        private void DrawLabel(int value, string label)
        {
            if (value != 0)
            {
                EditorGUILayout.LabelField(label + " : " + value);
                _hasDrawnSomething = true;
            }
        }

        private void OnEnable()
        {
            _refresher = target as EasyRefresher;
        }
    }
}
