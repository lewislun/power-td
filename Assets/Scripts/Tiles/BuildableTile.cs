using UnityEngine;

public class BuildableTile : MonoBehaviour, ITile {

    public bool IsPassable { get => building == null; } // TODO: check if building is passable (trap)
    public bool IsBuildable { get => building == null; }

    private Building building;

    public bool Build(Building building) {
        // TODO: check currency

        if (this.building != null) {
            Debug.LogError("Already built");
            return false;
        } else if (!building.BuildAt(this)) {
            return false;
        }

        this.building = building;

        PathFinder.Instance.UpdatePaths();
        return true;
    }
}