using System.Collections;
using UnityEngine;

public class DestinationTile : MonoBehaviour, ITile {

    public bool IsPassable { get; } = true;
    public bool IsBuildable { get; } = false;

}
