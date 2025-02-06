using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(HealthMeter))]
public class Enemy : MonoBehaviour, IDamageable {

    private HealthMeter healthMeter;

    void Start() {
        healthMeter = GetComponent<HealthMeter>();
        healthMeter.OnValueZero.AddListener(Die);
    }

    public void TakeDamage(float damage) {
        healthMeter.AddDelta(-damage);
    }

    public void HealDamage(float healAmount) {
        healthMeter.AddDelta(healAmount);
    }

    public void Die() {
        Destroy(gameObject);
    }
}
