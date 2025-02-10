using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DirectionalLaser : MonoBehaviour, IPausable {

    [Header("Attributes")]
    public float EnergyPerSec = 1f;
    public float DamagePerSec = 1f;
    public Vector2 Direction = Vector2.right;
    public Transform BlockerToIgnore; // to avoid hitting self

    [Header("Info")]
    [field: SerializeField] public bool IsPaused { get; private set; }

    public void Pause() => IsPaused = true;
    public void Unpause() => IsPaused = false;

    private LineRenderer lineRenderer;
    private LayerMask laserEndHitMask;
    private LayerMask damageHitMask;

    protected void Awake() {
        laserEndHitMask = LayerMask.GetMask(Layer.LaserBlocker);
        damageHitMask = LayerMask.GetMask(Layer.Enemy);
    }

    protected void Start() {
        lineRenderer = GetComponent<LineRenderer>();
    }

    protected void Update() {
        if (IsPaused) {
            return;
        }

        Vector3 laserEndPos = GetLaserEndPos();
        if (SpendEnergy()) {
            RenderLaser(laserEndPos);
            Shoot(laserEndPos);
        } else {
            HideLaser();
        }
    }

    protected bool SpendEnergy() {
        return EnergyMeter.Instance.SubtractIfEnough(EnergyPerSec * Time.deltaTime);
    }

    protected void Shoot(Vector3 laserEndPos) {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Direction, Vector2.Distance(laserEndPos, transform.position), damageHitMask);
        foreach (RaycastHit2D hit in hits) {
            if (hit.collider != null && hit.collider.gameObject != gameObject) {
                hit.collider.GetComponent<IDamageable>()?.Damage(DamagePerSec * Time.deltaTime);
            }
        }
    }

    protected void RenderLaser(Vector3 laserEndPos) {
        laserEndPos -= transform.position;
        laserEndPos.x /= transform.lossyScale.x;
        laserEndPos.y /= transform.lossyScale.y;
        laserEndPos.z = 0;
        lineRenderer.SetPosition(1, laserEndPos);
    }

    protected void HideLaser() {
        lineRenderer.SetPosition(1, Vector3.zero);
    }

    protected Vector3 GetLaserEndPos() {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Direction, 100f, laserEndHitMask);
        foreach (RaycastHit2D hit in hits) {
            if (hit.collider != null && hit.collider.transform != BlockerToIgnore) {
                return hit.point;
            }
        }
        return transform.position + (Vector3)Direction * 100;
    }
}
