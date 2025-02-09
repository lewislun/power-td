using UnityEngine;

[RequireComponent(typeof(TargetFinder))]
public class ProjectileShooter : MonoBehaviour, IPausable {
    
    [Header("References")]
    public GameObject ProjectilePrefab;

    [Header("Attributes")]
    public float ProjectilePerSec = 1f;

    [Header("Information")]
    [SerializeField] private float timeSinceLastShot = 0f;
    [field: SerializeField] public bool IsPaused { get; private set; }

    private TargetFinder targetFinder;

    public void Pause() => IsPaused = true;
    public void Unpause() => IsPaused = false;

    private void Start() {
        targetFinder = GetComponent<TargetFinder>();
        targetFinder.OnTargetChanged.AddListener(OnTargetChanged);

        if (ProjectilePrefab == null) {
            Debug.LogError("ProjectilePrefab is not set in " + name);
        }
    }

    public bool Shoot() {
        // TODO: specific position to spawn projectile
        if (targetFinder.CurrentTarget == null) {
            return false;
        }
        GameObject projectile = Instantiate(ProjectilePrefab, transform.position, Quaternion.identity, transform);
        projectile.GetComponent<IProjectile>().Target = targetFinder.CurrentTarget;
        return true;
    }

    private void OnTargetChanged() {
        // TODO: rotate towards target
    }

    private void Update() {
        if (IsPaused) {
            return;
        }

        timeSinceLastShot += Time.deltaTime;
        if (timeSinceLastShot >= 1 / ProjectilePerSec) {
            // TODO: check if rotation is done yet
            if (Shoot()) {
                timeSinceLastShot = 0;
            }
        }
    }
}
