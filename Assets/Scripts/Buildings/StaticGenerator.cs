using UnityEngine;

public class StaticGenerator : MonoBehaviour {
    [Header("Attributes")]
    [field: SerializeField] public float EnergyPerSec { get; protected set; } = 1f;

    private void Update() {
        EnergyMeter.Instance.Add(EnergyPerSec * Time.deltaTime);
    }
}
