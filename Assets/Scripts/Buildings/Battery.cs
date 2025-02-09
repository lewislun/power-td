using UnityEngine;

public class Battery : MonoBehaviour, IPausable {

    [Header("Attributes")]
    [field: SerializeField] public ModifiableFloat EnergyCapacity { get; protected set; } = new(1000f);

    [Header("Information")]
    [field: SerializeField] public bool IsPaused { get; private set; }

    public void Pause() => IsPaused = true;
    public void Unpause() => IsPaused = false;

    private int capacityId = -1;

    private void Update() {
        if (capacityId != -1 || IsPaused) {
            return;
        }

        UpdateCapacity();
        EnergyCapacity.OnValueChanged.AddListener(UpdateCapacity);
    }

    private void OnDestroy() {
        RemoveCapacity();
    }

    private void UpdateCapacity() {
        RemoveCapacity();
        capacityId = EnergyMeter.Instance.AddCapacity(EnergyCapacity.Value);
    }

    private void RemoveCapacity() {
        if (capacityId != -1) {
            EnergyMeter.Instance.RemoveCapacity(capacityId);
        }
    }
}
