using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(HealthMeter))]
public class Enemy : MonoBehaviour {

    void Start() {
        HealthMeter healthMeter = GetComponent<HealthMeter>();
        healthMeter.OnValueZero.AddListener(Die);
    }

    private void Die() {
        Destroy(gameObject);
    }
}
