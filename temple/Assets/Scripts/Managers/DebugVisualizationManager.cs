using UnityEngine;

/// <summary>
/// Manages debug visualization toggles for navigation nodes
/// Allows toggling paths and intermediate node display in the editor
/// </summary>
public class DebugVisualizationManager : MonoBehaviour
{
    public static DebugVisualizationManager Instance { get; private set; }

    [SerializeField] private bool drawNavigationPaths = true;
    [SerializeField] private bool drawIntermediateNodes = true;
    [SerializeField] private bool drawNodeLabels = true;
    [SerializeField] private bool drawConnectionLabels = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public bool DrawNavigationPaths
    {
        get => drawNavigationPaths;
        set => drawNavigationPaths = value;
    }

    public bool DrawIntermediateNodes
    {
        get => drawIntermediateNodes;
        set => drawIntermediateNodes = value;
    }

    public bool DrawNodeLabels
    {
        get => drawNodeLabels;
        set => drawNodeLabels = value;
    }

    public bool DrawConnectionLabels
    {
        get => drawConnectionLabels;
        set => drawConnectionLabels = value;
    }

#if UNITY_EDITOR
    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 200));
        GUILayout.Label("Debug Visualization", GUI.skin.box);

        drawNavigationPaths = GUILayout.Toggle(drawNavigationPaths, "Draw Navigation Paths");
        drawIntermediateNodes = GUILayout.Toggle(drawIntermediateNodes, "Draw Intermediate Nodes");
        drawNodeLabels = GUILayout.Toggle(drawNodeLabels, "Draw Node Labels");
        drawConnectionLabels = GUILayout.Toggle(drawConnectionLabels, "Draw Connection Labels");

        GUILayout.EndArea();
    }
#endif
}
