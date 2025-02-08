using UnityEngine;

public class Building : MonoBehaviour {

    [Header("Attributes")]
    [field:SerializeField] public bool IsPassable { get; private set; } = false;
    [field:SerializeField] public float Cost { get; private set; } = 100;

    [Header("Debug")]
    [field:SerializeField, ReadOnly] public BuildableTile Tile { get; private set; }

    private void Awake() {
        Pausable.Pause(gameObject);
    }

    public bool IsBuildableAt(BuildableTile tile) {
        if (tile == null) {
            return false;
        }
        return tile.IsBuildable;
    }

    // Should only be called by BuildableTile
    public bool Build(BuildableTile tile) {
        if (Tile != null) {
            Debug.LogError("Already built");
            return false;
        } else if (!IsBuildableAt(tile)) {
            Debug.LogError($"Cannot build at {tile}");
            return false;
        } else if (!CurrencyMeter.Instance.CanAfford(Cost)) {
            Debug.LogError("Not enough currency");
            return false;
        }

        if (!tile.Place(this)) {
            return false;
        }
        // check if building is blocking paths
        if (!IsPassable) {
            PathFinder.Instance.UpdatePaths();
            if (!PathFinder.Instance.AreDestinationsReachableFromAllSpawners()) {
                Debug.LogError("Building is blocking paths");
                PathFinder.Instance.UpdatePaths();
                tile.RemoveBuilding();
                return false;
            }
        }

        Tile = tile;
        transform.position = tile.transform.position;
        CurrencyMeter.Instance.Spend(Cost);
        Pausable.Unpause(gameObject);
        return true;
    }
}
