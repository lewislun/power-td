using UnityEngine;
using UnityEngine.Events;

public abstract class BuildingEffect : MonoBehaviour {

    [Header("Events")]
    [field:SerializeField] public UnityEvent OnApply { get; private set; } = new();
    [field:SerializeField] public UnityEvent OnRemove { get; private set; } = new();

    [Header("Info")]
    [field: SerializeField, ReadOnly] public Building Building { get; private set; }

    public virtual void Apply(Building building) {
        Building = building;
        building.Effects.Add(this);
        OnApply.Invoke();
    }

    public virtual void Remove() {
        OnRemove.Invoke();
        Building.Effects.Remove(this);
        Destroy(gameObject);
    }
}
