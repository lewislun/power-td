using UnityEngine;

public class ObstacleTile : MonoBehaviour, ITile {

    public bool IsPassable { get; } = false;
    public bool IsBuildable { get; } = false;

}