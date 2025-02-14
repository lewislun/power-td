using UnityEngine;

public class Cannon : Building {

    [field: SerializeField] public ModifiableFloat Radius { get; protected set; } = new(2f);
    [field: SerializeField] public ModifiableFloat Damage { get; protected set; } = new(75f);
    [field: SerializeField] public ModifiableFloat ProjectilePerSec { get; protected set; } = new(1f);

    protected void Start() {
        // initialize values
        Radius.OnValueChanged.Invoke(Radius.Value);
        Damage.OnValueChanged.Invoke(Damage.Value);
        ProjectilePerSec.OnValueChanged.Invoke(ProjectilePerSec.Value);
    }

}
