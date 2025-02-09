using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ModifiableFloat {

    [field:SerializeField] public float Base { get; private set; } = 0f;
    [field:SerializeField] public float Min { get; private set; } = 0f;
    [field:SerializeField] public float Max { get; private set; } = 999999f;
    [field:SerializeField, ReadOnly] public float Value { get; private set; }

    public Dictionary<int, float> AdditiveModifiers { get; private set; } = new();
    public Dictionary<int, float> MultiplicativeModifiers { get; private set; } = new();

    public UnityEvent OnValueChanged = new();

    private int nextModifierId = 0;

    public ModifiableFloat(float Base = 0f, float Min = 0f, float Max = float.MaxValue) {
        this.Base = Base;
        this.Min = Min;
        this.Max = Max;
        UpdateValue();
    }

    public void SetBase(float value) {
        Base = value;
        UpdateValue();
    }

    public void SetMax(float value) {
        Max = value;
        UpdateValue();
    }

    public void SetMin(float value) {
        Min = value;
        UpdateValue();
    }

    public void UpdateValue() {
        float prevValue = Value;
        Value = Base;
        foreach (var modifier in AdditiveModifiers.Values) {
            Value += modifier;
        }
        foreach (var modifier in MultiplicativeModifiers.Values) {
            Value *= modifier;
        }
        Value = Mathf.Clamp(Value, Min, Max);
        if (prevValue != Value) {
            OnValueChanged.Invoke();
        }
    }

    public int AddAdditiveModifier(float modifier) {
        int id = nextModifierId++;
        AdditiveModifiers[id] = modifier;
        UpdateValue();
        return id;
    }

    public void RemoveModifier(int id) {
        if (!AdditiveModifiers.Remove(id) && !MultiplicativeModifiers.Remove(id)) {
            Debug.LogError("Modifier not found");
            return;
        }
        UpdateValue();
    }


}
