using UnityEngine;

public class BuildableTile : MonoBehaviour, ITile {

    [Header("Debug")]
    [field:SerializeField, ReadOnly] public Building Building { get; private set; }

    public bool IsPassable { get => Building == null || Building.IsPassable; }
    public bool IsBuildable { get => Building == null; }

    public bool Place(Building building) {
        if (Building != null) {
            Debug.LogError("Already built");
            return false;
        }

        Building = building;
        return true;
    }

    public bool RemoveBuilding() {
        if (Building == null) {
            Debug.LogError("No building to remove");
            return false;
        }

        Building = null;
        return true;
    }
}