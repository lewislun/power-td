using UnityEngine;
using UnityEngine.Events;

public abstract class BuildingEffect : MonoBehaviour {

    [Header("Events")]
    [field:SerializeField] public UnityEvent OnApply { get; private set; } = new();
    [field:SerializeField] public UnityEvent OnRemove { get; private set; } = new();

    [Header("Info")]
    [field: SerializeField, ReadOnly] public Building Building { get; private set; }

    public virtual void Apply(Building building) {
        if (building.EffectByType.ContainsKey(GetType())) {
            building.EffectByType[GetType()].Reapply(this);
            Destroy(gameObject);
            return;
        }

        Building = building;
        building.EffectByType[GetType()] = this;
        OnApply.Invoke();
    }

    public abstract void Reapply(BuildingEffect otherEffect);

    public virtual void Remove() {
        OnRemove.Invoke();
        Building.EffectByType.Remove(GetType());
        Destroy(gameObject);
    }
}
