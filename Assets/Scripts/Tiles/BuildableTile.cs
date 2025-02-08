using NUnit.Framework;
using UnityEngine;

public class BuildableTile : MonoBehaviour, ITile {

    public bool IsPassable { get => building == null || building.IsPassable; }
    public bool IsBuildable { get => building == null; }

    private Building building;

    public bool Build(Building building) {
        // TODO: check currency

        if (this.building != null) {
            Debug.LogError("Already built");
            return false;
        } else if (!building.IsBuildableAt(this)) {
            return false;
        }

        this.building = building;
        if (!IsPassable) {
            PathFinder.Instance.UpdatePaths();
            if (!PathFinder.Instance.AreDestinationsReachableFromAllSpawners()) {
                Debug.LogError("Building is blocking paths");
                this.building = null;
                PathFinder.Instance.UpdatePaths();
                return false;
            }
        }

        if (!building.BuildAt(this)) {
            this.building = null;
            return false;
        }

        return true;
    }
}