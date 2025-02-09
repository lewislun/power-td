using UnityEngine;

public class StaticGenerator : MonoBehaviour, IPausable {
    [Header("Attributes")]
    [field: SerializeField] public float EnergyPerSec { get; protected set; } = 1f;

    [Header("Information")]
    [field: SerializeField] public bool IsPaused { get; private set; }

    public void Pause() => IsPaused = true;
    public void Unpause() => IsPaused = false;

    private void Update() {
        if (IsPaused) {
            return;
        }
        EnergyMeter.Instance.Add(EnergyPerSec * Time.deltaTime);
    }
}
