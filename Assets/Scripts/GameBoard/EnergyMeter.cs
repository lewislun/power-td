using UnityEngine;

public class EnergyMeter : Meter {

    [Header("Info")]
    [field: SerializeField] public ModifiableFloat FrozenEnergy { get; private set; } = new(0f);
    [field: SerializeField] public ModifiableFloat FrozenCapacity { get; private set; } = new(0f);

    public static EnergyMeter Instance { get; private set; }

    public int[] Freeze(float amount) {
        var energyRatio = (CurrentValue - FrozenEnergy.Value) / (MaxValue.Value - FrozenCapacity.Value);
        var energyId = FrozenEnergy.AddAdditiveModifier(amount * energyRatio);
        var capacityId = FrozenCapacity.AddAdditiveModifier(amount * (1 - energyRatio));
        return new int[] { energyId, capacityId };
    }

    public void Unfreeze(int[] ids) {
        FrozenEnergy.RemoveModifier(ids[0]);
        FrozenCapacity.RemoveModifier(ids[1]);
    }

    public override bool HasEnough(float amount) {
        return amount <= CurrentValue - FrozenEnergy.Value;
    }

    protected override void AddDelta(float delta) {
        if (delta == 0) return;

        float previousValue = CurrentValue;
        CurrentValue = Mathf.Clamp(CurrentValue + delta, FrozenEnergy.Value, MaxValue.Value - FrozenCapacity.Value);
        if (CurrentValue == previousValue) return;

        if (CurrentValue == 0) {
            OnValueZero.Invoke();
        } else if (CurrentValue == MaxValue.Value) {
            OnValueMax.Invoke();
        }
        OnValueChanged.Invoke(CurrentValue);
    }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Debug.LogError("Multiple EnergyMeter instances");
        }
    }
}
