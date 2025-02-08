using UnityEngine;

public class CurrencyMeter : Meter {
    public static CurrencyMeter Instance { get; private set; }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Debug.LogError("Multiple CurrencyMeter instances");
        }
    }

    public bool CanAfford(float amount) {
        if (amount < 0) {
            throw new System.ArgumentException("Spend amount cannot be negative");
        }
        return amount <= CurrentValue;
    }

    public bool Spend(float amount) {
        if (amount < 0) {
            Debug.LogError("Cannot spend negative amount");
            return false;
        } else if (!CanAfford(amount)) {
            Debug.LogError("Not enough currency");
            return false;
        }

        AddDelta(-amount);
        return true;
    }
}
