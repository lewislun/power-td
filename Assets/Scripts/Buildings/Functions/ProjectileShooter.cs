using UnityEngine;

public class ProjectileShooter : MonoBehaviour, IPausable {
    
    [Header("References")]
    public GameObject ProjectilePrefab;

    [Header("Attributes")]
    [field: SerializeField] public float ProjectilePerSec { get; set; } = 1f;
    [field: SerializeField] public float Damage { get; set; } = 75f;

    [Header("Information")]
    [field: SerializeField, ReadOnly] public float TimeSinceLastShot { get; private set; } = 0f;
    [field: SerializeField, ReadOnly] public bool IsAlignedWithTarget { get; set; } = false;
    [field: SerializeField, ReadOnly] public Transform Target { get; private set; }
    [field: SerializeField, ReadOnly] public bool IsPaused { get; private set; }

    public void Pause() => IsPaused = true;
    public void Unpause() => IsPaused = false;

    public void SetTarget(Transform target) {
        Target = target;
    }

    public bool Shoot() {
        if (Target == null || !IsAlignedWithTarget) {
            return false;
        }
        Transform parent = LevelManager.Instance.ProjectileParent.transform;
        Vector3 pos = new(transform.position.x, transform.position.y, parent.position.z);
        GameObject projectileObject = Instantiate(ProjectilePrefab, pos, Quaternion.identity, parent);
        IProjectile projectile = projectileObject.GetComponent<IProjectile>();
        projectile.SetTarget(Target);
        projectile.Damage = Damage;
        return true;
    }

    protected void Awake() {
        if (ProjectilePrefab == null) {
            Debug.LogError("ProjectilePrefab is not set in " + name);
        }
    }

    protected void Update() {
        if (IsPaused || !LevelManager.Instance.IsWaveActive) {
            return;
        }

        TimeSinceLastShot += Time.deltaTime;
        if (TimeSinceLastShot >= 1 / ProjectilePerSec) {
            if (Shoot()) {
                TimeSinceLastShot = 0;
            }
        }
    }
}
