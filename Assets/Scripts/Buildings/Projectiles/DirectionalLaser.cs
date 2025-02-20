using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DirectionalLaser : MonoBehaviour, IPausable {

    [Header("Attributes")]
    public float EnergyPerSec = 1f;
    public float DamagePerSec = 1f;
    public Vector2 Direction = Vector2.up;
    public Transform BlockerToIgnore; // to avoid hitting self

    [Header("Info")]
    [field: SerializeField] public bool IsPaused { get; private set; }

    public void Pause() {
        IsPaused = true;
        lineRenderer.enabled = false;
    }
    public void Unpause() {
        IsPaused = false;
        lineRenderer.enabled = true;
    }

    private LineRenderer lineRenderer;
    private LayerMask laserEndHitMask;
    private LayerMask damageHitMask;

    public void SetDirection(Vector2 direction) {
        Direction = direction;
    }

    protected void Awake() {
        laserEndHitMask = LayerMask.GetMask(Layer.LaserBlocker);
        damageHitMask = LayerMask.GetMask(Layer.Enemy);
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
        if (LevelManager.Instance.IsWaveActive) {
            return EnergyMeter.Instance.SubtractIfEnough(EnergyPerSec * Time.deltaTime);
        }
        return true;
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
        if (!lineRenderer.enabled) {
            lineRenderer.enabled = true;
        }
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, laserEndPos);
    }

    protected void HideLaser() {
        if (lineRenderer.enabled) {
            lineRenderer.enabled = false;
        }
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
