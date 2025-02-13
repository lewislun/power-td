using UnityEngine;

public class Cannon : Building {

    [field: SerializeField] public ModifiableFloat Radius { get; protected set; } = new(2f);

    protected void Start() {
        Radius.OnValueChanged.Invoke(Radius.Value);
    }

}
