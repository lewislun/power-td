using UnityEngine;
using UnityEngine.Events;

public class Meter : MonoBehaviour {
    [Header("Attributes")]
    public float MaxValue = 100;

    [Header("Information")]
    [field: SerializeField] public float CurrentValue { get; private set; } = 100;

    [Header("Events")]
    public UnityEvent OnValueZero;
    public UnityEvent OnValueMax;

    public void AddDelta(float delta) {
        CurrentValue = Mathf.Clamp(CurrentValue + delta, 0, MaxValue);
        if (CurrentValue == 0) {
            OnValueZero.Invoke();
        } else if (CurrentValue == MaxValue) {
            OnValueMax.Invoke();
        }
    }
}
