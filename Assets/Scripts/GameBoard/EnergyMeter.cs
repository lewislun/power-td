using UnityEngine;

public class EnergyMeter : Meter {
    public static EnergyMeter Instance { get; private set; }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Debug.LogError("Multiple EnergyMeter instances");
        }
    }

    public int AddCapacity(float amount) {
        return MaxValue.AddAdditiveModifier(amount);
    }

    public void RemoveCapacity(int id) {
        MaxValue.RemoveModifier(id);
    }
}
