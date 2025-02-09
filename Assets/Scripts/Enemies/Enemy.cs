using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(HealthMeter))]
[RequireComponent(typeof(PathNavigator))]
public class Enemy : MonoBehaviour, IDamageable {

    [Header("Attributes")]
    public float DamageToBase = 1f;
    public float currencyReward = 10f;

    private HealthMeter healthMeter;
    private PathNavigator pathNavigator;

    void Start() {
        healthMeter = GetComponent<HealthMeter>();
        healthMeter.OnValueZero.AddListener(Kill);
        pathNavigator = GetComponent<PathNavigator>();
        pathNavigator.OnReachedDestination.AddListener(OnReachDestination);
    }

    public void Damage(float damage) {
        healthMeter.Subtract(damage);
    }

    public void Heal(float healAmount) {
        healthMeter.Add(healAmount);
    }

    public void OnReachDestination() {
        // TODO: GameManager.Instance.TakeDamage(DamageToBase);
        Die();
    }

    public void Kill() {
        CurrencyMeter.Instance.Add(currencyReward);
        Die();
    }

    public void Die() {
        Destroy(gameObject);
    }
}
