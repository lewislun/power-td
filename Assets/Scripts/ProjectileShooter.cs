using UnityEngine;

[RequireComponent(typeof(TargetFinder))]
public class ProjectileShooter : MonoBehaviour {
    
    [Header("References")]
    public GameObject ProjectilePrefab;

    [Header("Attributes")]
    public float ProjectilePerSec = 1f;

    [Header("Information")]
    [SerializeField] private float timeSinceLastShot = 0f;
    
    private TargetFinder targetFinder;

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
        timeSinceLastShot += Time.deltaTime;
        if (timeSinceLastShot >= 1 / ProjectilePerSec) {
            // TODO: check if rotation is done yet
            if (Shoot()) {
                timeSinceLastShot = 0;
            }
        }
    }
}
