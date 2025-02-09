using UnityEngine;

public class Battery : MonoBehaviour, IPausable {

    [Header("Attributes")]
    [field: SerializeField] public ModifiableFloat EnergyCapacity { get; protected set; } = new(1000f);

    [Header("Information")]
    [field: SerializeField] public bool IsPaused { get; private set; }

    public void Pause() => IsPaused = true;
    public void Unpause() => IsPaused = false;

    private int modifierId = -1;

    private void Update() {
        if (modifierId != -1 || IsPaused) {
            return;
        }

        UpdateCapacity();
        EnergyCapacity.OnValueChanged.AddListener(UpdateCapacity);
    }

    private void OnDestroy() {
        RemoveCapacity();
    }

    private void UpdateCapacity() {
        if (modifierId != -1) {
            EnergyMeter.Instance.MaxValue.EditModifier(modifierId, EnergyCapacity.Value);
        } else {
            modifierId = EnergyMeter.Instance.MaxValue.AddAdditiveModifier(EnergyCapacity.Value);
        }
    }

    private void RemoveCapacity() {
        if (modifierId != -1) {
            EnergyMeter.Instance.MaxValue.RemoveModifier(modifierId);
        }
    }
}
