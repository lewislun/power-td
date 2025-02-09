using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class EnergyBar : MonoBehaviour {

    [Header("References")]
    [field: SerializeField] public RectTransform Fill { get; private set; }

    private RectTransform rectTransform;

    private void Awake() {
        if (Fill == null) {
            Debug.LogError("Fill is not set in EnergyBar script on " + gameObject.name);
            return;
        }
    }

    private void Start() {
        rectTransform = GetComponent<RectTransform>();
        EnergyMeter.Instance.OnValueChanged.AddListener(UpdateFill);
    }

    private void UpdateFill() {
        float energyPercentage = EnergyMeter.Instance.CurrentValue / EnergyMeter.Instance.MaxValue.Value;
        if (float.IsNaN(energyPercentage)) {
            return;
        }
        int width = (int)(rectTransform.rect.width * energyPercentage);
        Fill.sizeDelta = new Vector2(width, Fill.rect.height);
    }
}
