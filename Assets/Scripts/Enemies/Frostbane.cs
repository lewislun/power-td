using UnityEngine;

[RequireComponent(typeof(HealthMeter))]
[RequireComponent(typeof(PathNavigator))]
public class Frostbane : Enemy {

    public GameObject Explosion;
    [field: SerializeField] public ModifiableFloat ExplosionRadius { get; protected set; } = new(2f);

    public override void Die() {
        // detaches the explosion from the enemy
        Explosion.transform.parent = transform.parent;
        base.Die();
    }

}
