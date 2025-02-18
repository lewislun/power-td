using UnityEngine;

public class BuildingProgressDisplay : MonoBehaviour {

    [Header("References")]
    [field: SerializeField] public LineRenderer TopLineRenderer { get; private set; }
    [field: SerializeField] public LineRenderer BottomLineRenderer { get; private set; }
    [field: SerializeField] public LineRenderer LeftLineRenderer { get; private set; }
    [field: SerializeField] public LineRenderer RightLineRenderer { get; private set; }

    [Header("Attributes")]
    [field: SerializeField] public float Progress { get; private set; } = 0f;
    
    protected float topMaxX;
    protected float topMinX;
    protected float bottomMaxX;
    protected float bottomMinX;
    protected float leftMaxY;
    protected float leftMinY;
    protected float rightMaxY;
    protected float rightMinY;

    public void SetProgress(float progress) {
        Progress = progress;
        UpdateRenderers();
    }

    public void SetProgress(float value, float max) {
        SetProgress(value / max);
    }

    protected void Awake() {
        if (TopLineRenderer == null) {
            Debug.LogError("TopLineRenderer is not set");
        } else {
            topMinX = TopLineRenderer.GetPosition(0).x;
            topMaxX = TopLineRenderer.GetPosition(1).x;
        }
        if (BottomLineRenderer == null) {
            Debug.LogError("BottomLineRenderer is not set");
        } else {
            bottomMinX = BottomLineRenderer.GetPosition(0).x;
            bottomMaxX = BottomLineRenderer.GetPosition(1).x;
        }
        if (LeftLineRenderer == null) {
            Debug.LogError("LeftLineRenderer is not set");
        } else {
            leftMinY = LeftLineRenderer.GetPosition(0).y;
            leftMaxY = LeftLineRenderer.GetPosition(1).y;
        }
        if (RightLineRenderer == null) {
            Debug.LogError("RightLineRenderer is not set");
        } else {
            rightMinY = RightLineRenderer.GetPosition(0).y;
            rightMaxY = RightLineRenderer.GetPosition(1).y;
        }
    }

    protected void UpdateRenderers() {
        if (TopLineRenderer != null) {
            Vector3 pos0 = TopLineRenderer.GetPosition(0);
            pos0.x = Mathf.Lerp(topMinX, topMaxX, Progress);
            TopLineRenderer.SetPosition(0, pos0);
        }
        if (BottomLineRenderer != null) {
            Vector3 pos0 = BottomLineRenderer.GetPosition(0);
            pos0.x = Mathf.Lerp(bottomMinX, bottomMaxX, Progress);
            BottomLineRenderer.SetPosition(0, pos0);
        }
        if (LeftLineRenderer != null) {
            Vector3 pos0 = LeftLineRenderer.GetPosition(0);
            pos0.y = Mathf.Lerp(leftMinY, leftMaxY, Progress);
            LeftLineRenderer.SetPosition(0, pos0);
        }
        if (RightLineRenderer != null) {
            Vector3 pos0 = RightLineRenderer.GetPosition(0);
            pos0.y = Mathf.Lerp(rightMinY, rightMaxY, Progress);
            RightLineRenderer.SetPosition(0, pos0);
        }
    }
}
