using UnityEngine;

[RequireComponent(typeof(HealthMeter))]
[RequireComponent(typeof(PathNavigator))]
public class Frostbane : Enemy {

    [Header("Frostbane")]
    [field: SerializeField] public FreezeEffect FreezeEffect { get; protected set; }
    [field: SerializeField] public ModifiableFloat ExplosionRadius { get; protected set; } = new(2f);
    [field: SerializeField] public ModifiableFloat FreezeDuration { get; protected set; } = new(10f);

    public void Explode() {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, ExplosionRadius.Value, Vector2.zero, 10f, LayerMask.GetMask(Layer.Building));
        foreach (RaycastHit2D hit in hits) {
            if (hit.collider.TryGetComponent<Building>(out var building)) {
                var freezeEffectObject = Instantiate(FreezeEffect.gameObject, building.transform);
                var freezeEffect = freezeEffectObject.GetComponent<FreezeEffect>();
                freezeEffect.SetDuration(FreezeDuration.Value);
                freezeEffect.Apply(building);
            }
        }
    }

    protected override void Start() {
        base.Start();
        OnKill.AddListener(Explode);
    }

}
