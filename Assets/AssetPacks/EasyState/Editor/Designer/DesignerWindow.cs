using EasyState.Core.Utility;
using EasyState.Data.FileSync;
using EasyState.Editor.Designer;
using System.IO;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;

public class DesignerWindow : EditorWindow
{
    private DesignBootstrapper _bootstrapper;
    public static DesignerWindow Instance { get; private set; }
    public static bool IsOpen
    {
        get { return Instance != null; }
    }
    private void OnEnable()
    {
        FileWatcher.FileChanged += OnCreateGUI;
        Instance = this;
        OnCreateGUI();
    }

    [MenuItem("Window/Easy State/Designer")]
    public static void ShowWindow()
    {
        DesignerWindow wnd = GetWindow<DesignerWindow>("Designer", typeof(SceneView));
        var icon = AssetDatabase.LoadAssetAtPath<Texture2D>(Path.Combine(FilePaths.EditorImageFolder, "Designer-dark.png"));
        if (EditorGUIUtility.isProSkin)
        {
            icon = AssetDatabase.LoadAssetAtPath<Texture2D>(Path.Combine(FilePaths.EditorImageFolder, "Designer.png"));
        }
        wnd.titleContent = new GUIContent("Designer", icon);

    }
    public void OnCreateGUI()
    {
        if (rootVisualElement == null)
        {
            return;
        }
        this.SetAntiAliasing(4);
        if (_bootstrapper != null)
        {
            _bootstrapper.Dispose();
        }

        _bootstrapper = new DesignBootstrapper(rootVisualElement);
    }

    private void OnDisable()
    {
        if (_bootstrapper != null)
        {
            _bootstrapper.SaveDesignState();
            _bootstrapper.Dispose();
            _bootstrapper = null;
        }
        FileWatcher.FileChanged -= OnCreateGUI;
    }

    private void OnLostFocus()
    {
        System.Threading.Tasks.Task.Run(() => _bootstrapper.SaveDesignState()).ConfigureAwait(false);
    }
}