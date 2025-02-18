using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Selectable))]
public class Building : MonoBehaviour, IPausable {

    [Header("Attributes")]
    [field: SerializeField] public bool IsPassable { get; private set; } = false;
    [field: SerializeField] public float Cost { get; private set; } = 100;

    [Header("Events")]
    public UnityEngine.Events.UnityEvent OnBuilt;

    [Header("Info")]
    [field: SerializeField, ReadOnly] public BuildableTile Tile { get; private set; }
    [field: SerializeField, ReadOnly] public bool IsPaused { get; private set; }

    public HashSet<BuildingEffect> Effects { get; private set; } = new();

    public void Pause() => IsPaused = true;
    public void Unpause() => IsPaused = false;

    protected virtual void Awake() {
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
        } else if (!CurrencyMeter.Instance.HasEnough(Cost)) {
            Debug.Log("Not enough currency");
            return false;
        }

        if (!tile.Place(this)) {
            return false;
        }
        // check if building is blocking paths
        if (!IsPassable) {
            PathFinder.Instance.UpdatePaths();
            if (!PathFinder.Instance.AreDestinationsReachableFromAllSpawners()) {
                Debug.Log("Building is blocking paths");
                PathFinder.Instance.UpdatePaths();
                tile.RemoveBuilding();
                return false;
            }
        }

        Tile = tile;
        transform.position = tile.transform.position;
        CurrencyMeter.Instance.Subtract(Cost);
        Pausable.Unpause(gameObject);
        OnBuilt.Invoke();
        return true;
    }

}
