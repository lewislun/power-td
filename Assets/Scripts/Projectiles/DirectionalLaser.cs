using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DirectionalLaser : MonoBehaviour {

    [Header("Attributes")]
    public Vector2 Direction = Vector2.right;
    public LayerMask HitMask;

    private LineRenderer lineRenderer;

    private void Start() {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update() {
        UpdateLaserRendering();
    }

    private void UpdateLaserRendering() {
        lineRenderer.SetPosition(0, Vector3.zero);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Direction, Mathf.Infinity, HitMask);
        if (hit.collider != null) {
            Vector3 laserEnd = (Vector3)hit.point - transform.position;
            laserEnd.x /= transform.lossyScale.x;
            laserEnd.y /= transform.lossyScale.y;
            laserEnd.z = 0;
            lineRenderer.SetPosition(1, laserEnd);
        } else {
            lineRenderer.SetPosition(1, Vector3.zero + (Vector3)Direction * 100);
        }
    }
}
