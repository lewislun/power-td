using UnityEngine;

public class BuildableTile : MonoBehaviour, ITile {

    public bool IsPassable { get => tower == null; } // TODO: check if building is passable (trap)
    public bool IsBuildable { get => tower == null; }

    private Tower tower;

    public void Build(Tower tower) {
        if (this.tower != null) {
            Debug.LogError("Already built");
            return;
        }

        this.tower = tower;
    }
}