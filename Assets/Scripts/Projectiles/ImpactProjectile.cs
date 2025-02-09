using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class ImpactProjectile : MonoBehaviour, IProjectile {

    [Header("Attributes")]
    [field:SerializeField] public float TravelSpeed { get; set; } = 5f;
    [field:SerializeField] public float Damage { get; set; } = 10f;
    [field:SerializeField] public Transform Target { get; set; }

    private void Update() {
        if (Target == null) {
            DestroyProjectile();
            return;
        }
        Vector3 direction = Target.position - transform.position;
        direction.z = 0;
        transform.position += Time.deltaTime * TravelSpeed * direction.normalized;
    }

    public void DestroyProjectile() {
        Destroy(gameObject);
    }

    private void Hit(IDamageable damageable) {
        damageable.Damage(Damage);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.TryGetComponent<IDamageable>(out var damageable)) {
            Hit(damageable);
        }
        DestroyProjectile();
    }
}
