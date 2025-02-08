using UnityEngine;
using UnityEngine.Events;

public class Meter : MonoBehaviour {
    [Header("Attributes")]
    [field: SerializeField] public virtual float MaxValue { get; protected set; } = 100;

    [Header("Information")]
    [field: SerializeField] public virtual float CurrentValue { get; protected set; } = 100;

    [Header("Events")]
    public UnityEvent OnValueZero;
    public UnityEvent OnValueMax;
    public UnityEvent OnValueChanged;

    public void AddDelta(float delta) {
        if (delta == 0) return;

        CurrentValue = Mathf.Clamp(CurrentValue + delta, 0, MaxValue);
        if (CurrentValue == 0) {
            OnValueZero.Invoke();
        } else if (CurrentValue == MaxValue) {
            OnValueMax.Invoke();
        }
        OnValueChanged.Invoke();
    }
}
