using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CircleLine : MonoBehaviour {
    [Header("Attributes")]
    [field:SerializeField] public float Radius { get; private set; } = 1f;
    [field:SerializeField] public int PointCountPerDiameterUnit { get; private set; } = 10;

    protected LineRenderer lineRenderer;

    public void SetRadiusWithTileRange(float range) {
        SetRadius((range + 0.5f) / transform.lossyScale.x);
    }

    public void SetRadius(float radius) {
        Radius = radius;
        UpdateLine();
    }

    public void UpdateLine() {
        int segPointCount = (int)(PointCountPerDiameterUnit * Radius * 2 * Mathf.PI) / 4;
        int pointCount = segPointCount * 4;
        lineRenderer.positionCount = pointCount;
        for (int i = 0; i < segPointCount; i++) {
            float angle = 2 * Mathf.PI * i / pointCount;
            float x = Radius * Mathf.Cos(angle);
            float y = Radius * Mathf.Sin(angle);
            lineRenderer.SetPosition(i, new Vector3(x, y, 0));
            lineRenderer.SetPosition(segPointCount * 2 - i - 1, new Vector3(-x, y, 0));
            lineRenderer.SetPosition(i + segPointCount * 2, new Vector3(-x, -y, 0));
            lineRenderer.SetPosition(pointCount - i - 1, new Vector3(x, -y, 0));
        }
    }

    protected void Awake() {
        lineRenderer = GetComponent<LineRenderer>();
    }

    protected void Start() {
        UpdateLine();
    }

    protected void OnValidate() {
        lineRenderer = GetComponent<LineRenderer>();
        UpdateLine();
    }
}