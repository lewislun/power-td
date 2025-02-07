using UnityEngine;

public interface ITile {
    public bool IsPassable { get; set; }
    public bool IsBuildable { get; set; }
}