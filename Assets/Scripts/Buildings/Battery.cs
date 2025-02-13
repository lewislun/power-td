using UnityEngine;

public class Battery : Building {

    [field: SerializeField] public ModifiableFloat EnergyCapacity { get; protected set; } = new(1000f);

    protected int modifierId = -1;

    protected void Update() {
        if (modifierId != -1 || IsPaused) {
            return;
        }

        UpdateCapacity(EnergyCapacity.Value);
        EnergyCapacity.OnValueChanged.AddListener(UpdateCapacity);
    }

    protected void OnDestroy() {
        RemoveCapacity();
    }

    protected void UpdateCapacity(float newValue) {
        if (modifierId != -1) {
            EnergyMeter.Instance.MaxValue.EditModifier(modifierId, newValue);
        } else {
            modifierId = EnergyMeter.Instance.MaxValue.AddAdditiveModifier(newValue);
        }
    }

    protected void RemoveCapacity() {
        if (modifierId != -1) {
            EnergyMeter.Instance.MaxValue.RemoveModifier(modifierId);
        }
    }
}
