using UnityEngine;

public class ProjectileShooter : MonoBehaviour, IPausable {
    
    [Header("References")]
    public GameObject ProjectilePrefab;

    [Header("Attributes")]
    public float ProjectilePerSec = 1f;

    [Header("Information")]
    [SerializeField, ReadOnly] private float timeSinceLastShot = 0f;
    [field: SerializeField, ReadOnly] public Transform Target { get; private set; }
    [field: SerializeField, ReadOnly] public bool IsPaused { get; private set; }

    public void Pause() => IsPaused = true;
    public void Unpause() => IsPaused = false;

    public void SetTarget(Transform target) {
        Target = target;
    }

    protected void Start() {
        if (ProjectilePrefab == null) {
            Debug.LogError("ProjectilePrefab is not set in " + name);
        }
    }

    public bool Shoot() {
        if (Target == null) {
            return false;
        }
        Transform parent = LevelManager.Instance.ProjectileParent.transform;
        Vector3 pos = new(transform.position.x, transform.position.y, parent.position.z);
        GameObject projectile = Instantiate(ProjectilePrefab, pos, Quaternion.identity, parent);
        projectile.GetComponent<IProjectile>().Target = Target;
        return true;
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
