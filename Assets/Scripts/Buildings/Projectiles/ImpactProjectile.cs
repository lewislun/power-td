using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class ImpactProjectile : MonoBehaviour, IProjectile {

    [Header("Attributes")]
    [field: SerializeField] public float TravelSpeed { get; set; } = 5f;
    [field: SerializeField] public float Damage { get; set; } = 10f;

    [Header("Events")]
    [SerializeField] protected UnityEvent<Transform> onTargetChanged = new();
    public UnityEvent<Transform> OnTargetChanged { get => onTargetChanged; }
    [SerializeField] protected UnityEvent<Transform> onTargetHit = new();
    public UnityEvent<Transform> OnTargetHit { get => onTargetHit; }

    [Header("Info")]
    [field: SerializeField, ReadOnly] public Transform Target { get; protected set; }
    [field: SerializeField, ReadOnly] public Vector3 Direction { get; protected set; } = Vector3.zero;

    protected SpriteRenderer spriteRenderer;
    protected CircleCollider2D circleCollider;

    public void SetTarget(Transform target) {
        Target = target;
        if (Target != null) {
            var direction = (Target.position - transform.position).normalized;
            direction.z = transform.position.z;
            Direction = direction;
            float angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg - 90;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        OnTargetChanged.Invoke(target);
    }

    public void DestroyProjectile(bool shouldPlayEffect) {
        if (shouldPlayEffect) {
            spriteRenderer.enabled = false;
            circleCollider.enabled = false;
            TravelSpeed = 0;
            SetTarget(null);
            Destroy(gameObject, 3f);
        } else {
            Destroy(gameObject);
        }
    }

    protected void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    protected void Update() {
        transform.position += Time.deltaTime * TravelSpeed * Direction;
    }

    protected void Hit(IDamageable damageable) {
        damageable.Damage(Damage);
        OnTargetHit.Invoke(Target);
    }

    protected void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.TryGetComponent<IDamageable>(out var damageable)) {
            Hit(damageable);
            DestroyProjectile(true);
        }
    }
}
