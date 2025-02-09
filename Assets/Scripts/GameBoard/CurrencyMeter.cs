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
}
