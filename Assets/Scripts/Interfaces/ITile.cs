using UnityEngine;

public interface ITile {
    public bool IsPassable { get; }
    public bool IsBuildable { get; }
    public Transform transform { get; }
}