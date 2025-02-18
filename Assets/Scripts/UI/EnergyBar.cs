using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class EnergyBar : MonoBehaviour {

    [Header("References")]
    [field: SerializeField] public RectTransform Fill { get; private set; }
    [field: SerializeField] public RectTransform FrozenEnergyFill { get; private set; }
    [field: SerializeField] public RectTransform FrozenCapacityFill { get; private set; }

    private RectTransform rectTransform;

    private void Awake() {
        if (Fill == null) {
            Debug.LogError("Fill is not set in EnergyBar script on " + gameObject.name);
        } else if (FrozenEnergyFill == null) {
            Debug.LogError("FrozenEnergyFill is not set in EnergyBar script on " + gameObject.name);
        } else if (FrozenCapacityFill == null) {
            Debug.LogError("FrozenCapacityFill is not set in EnergyBar script on " + gameObject.name);
        }
    }

    private void Start() {
        rectTransform = GetComponent<RectTransform>();
        EnergyMeter.Instance.OnValueChanged.AddListener(UpdateFill);
        EnergyMeter.Instance.FrozenEnergy.OnValueChanged.AddListener(UpdateFrozenEnergyFill);
        EnergyMeter.Instance.FrozenCapacity.OnValueChanged.AddListener(UpdateFrozenCapacityFill);
        UpdateFill(EnergyMeter.Instance.CurrentValue);
        UpdateFrozenEnergyFill(EnergyMeter.Instance.FrozenEnergy.Value);
        UpdateFrozenCapacityFill(EnergyMeter.Instance.FrozenCapacity.Value);
    }

    private void UpdateFill(float value) {
        float ratio = value / EnergyMeter.Instance.MaxValue.Value;
        if (float.IsNaN(ratio)) {
            return;
        }
        int width = (int)(rectTransform.rect.width * ratio);
        Fill.sizeDelta = new Vector2(width, Fill.rect.height);
    }

    private void UpdateFrozenEnergyFill(float value) {
        float ratio = value / EnergyMeter.Instance.MaxValue.Value;
        if (float.IsNaN(ratio)) {
            return;
        }
        int width = (int)(rectTransform.rect.width * ratio);
        FrozenEnergyFill.sizeDelta = new Vector2(width, FrozenEnergyFill.rect.height);
    }

    private void UpdateFrozenCapacityFill(float value) {
        float ratio = value / EnergyMeter.Instance.MaxValue.Value;
        if (float.IsNaN(ratio)) {
            return;
        }
        int width = (int)(rectTransform.rect.width * ratio);
        FrozenCapacityFill.sizeDelta = new Vector2(width, FrozenCapacityFill.rect.height);
    }
}
