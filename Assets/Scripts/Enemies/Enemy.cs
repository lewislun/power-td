using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(HealthMeter))]
[RequireComponent(typeof(PathNavigator))]
public class Enemy : MonoBehaviour, IDamageable {

    [Header("Attributes")]
    public float DamageToBase = 1f;
    public float currencyReward = 10f;

    [field: SerializeField] public UnityEvent OnDeath { get; private set; } = new();
    [field: SerializeField] public UnityEvent OnKill { get; private set; } = new();

    protected HealthMeter healthMeter;
    protected PathNavigator pathNavigator;

    public virtual void Damage(float damage) {
        healthMeter.Subtract(damage);
    }

    public virtual void Heal(float healAmount) {
        healthMeter.Add(healAmount);
    }

    public void ReachDestination() {
        // TODO: GameManager.Instance.TakeDamage(DamageToBase);
        Die();
    }

    public void Kill() {
        OnKill.Invoke();
        CurrencyMeter.Instance.Add(currencyReward);
        Die();
    }

    public virtual void Die() {
        OnDeath.Invoke();
        Destroy(gameObject);
    }

    protected virtual void Awake() {
        healthMeter = GetComponent<HealthMeter>();
        pathNavigator = GetComponent<PathNavigator>();
    }
}
