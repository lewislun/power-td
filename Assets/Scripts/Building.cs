using UnityEngine;

public class Building : MonoBehaviour {

    [Header("Attributes")]
    [field:SerializeField] public bool IsPassable { get; private set; } = false;

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
    public bool BuildAt(BuildableTile tile) {
        if (Tile != null) {
            Debug.LogError("Already built");
            return false;
        }

        Tile = tile;
        transform.position = tile.transform.position;
        Pausable.Unpause(gameObject);
        return true;
    }
}
