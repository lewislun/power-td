using UnityEngine;
using UnityEngine.Events;

public class Meter : MonoBehaviour {
    [Header("Attributes")]
    [field: SerializeField] public ModifiableFloat MaxValue { get; protected set; } = new();

    [Header("Information")]
    [field: SerializeField] public virtual float CurrentValue { get; protected set; } = 100f;

    [Header("Events")]
    public UnityEvent OnValueZero;
    public UnityEvent OnValueMax;
    public UnityEvent OnValueChanged;

    public bool HasEnough(float amount) {
        return amount <= CurrentValue;
    }

    public bool Add(float amount) {
        if (amount < 0) {
            Debug.LogError("Cannot add negative amount");
            return false;
        } else if (CurrentValue == MaxValue.Value) {
            return false;
        }

        AddDelta(amount);
        return true;
    }

    public bool Subtract(float amount) {
        if (amount < 0) {
            Debug.LogError("Cannot subtract negative amount");
            return false;
        }

        AddDelta(-amount);
        return true;
    }

    protected void AddDelta(float delta) {
        if (delta == 0) return;

        float previousValue = CurrentValue;
        CurrentValue = Mathf.Clamp(CurrentValue + delta, 0, MaxValue.Value);
        if (CurrentValue == previousValue) return;

        if (CurrentValue == 0) {
            OnValueZero.Invoke();
        } else if (CurrentValue == MaxValue.Value) {
            OnValueMax.Invoke();
        }
        OnValueChanged.Invoke();
    }

    private void OnValidate() {
        MaxValue.UpdateValue();
        CurrentValue = Mathf.Clamp(CurrentValue, 0, MaxValue.Value);
    }

    private void Start() {
        MaxValue.UpdateValue();
    }
}
