using UnityEngine;

public class FreezeEffect : BuildingTimedEffect {

    [Header("Attributes")]
    [field: SerializeField] public float Progress { get; private set; } = 0f;

    public override void Apply(Building building) {
        base.Apply(building);
        Debug.Log("FreezeEffect applied to " + building.gameObject.name);
        Pausable.Pause(building.gameObject);
    }

    public override void Remove() {
        base.Remove();
        Pausable.Unpause(Building.gameObject);
    }
}
