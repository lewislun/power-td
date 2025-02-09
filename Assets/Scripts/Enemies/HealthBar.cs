using UnityEngine;

public class HealthBar : MonoBehaviour {

    [Header("References")]
    [field: SerializeField] public HealthMeter HealthMeter { get; private set; }
    [field: SerializeField] public Transform Fill { get; private set; }

    private void Start() {
        if (HealthMeter == null) {
            Debug.LogError("HealthMeter is not set in HealthBar script on " + gameObject.name);
            return;
        } else if (Fill == null) {
            Debug.LogError("Fill is not set in HealthBar script on " + gameObject.name);
            return;
        }

        HealthMeter.OnValueChanged.AddListener(UpdateFill);
    }

    private void UpdateFill() {
        Fill.localScale = new Vector3(HealthMeter.CurrentValue / HealthMeter.MaxValue.Value, 1, 1);
        Fill.transform.localPosition = new Vector3((Fill.localScale.x - 1) / 2, 0, 0);
    }

}
