using UnityEngine;
using UnityEngine.Events;

public class PathNavigator : MonoBehaviour {

    [Header("Attributes")]
    public float Speed = 1f;

    [Header("Events")]
    public UnityEvent OnReachedDestination;

    [Header("Debug")]
    [ReadOnly] public ITile NextTile;

    public void SetNextTile(ITile tile) {
        NextTile = tile;
    }

    private void Update() {
        MoveTowardsNextTile();
    }

    private void MoveTowardsNextTile() {
        if (NextTile == null) {
            return;
        }

        Vector3 direction = NextTile.transform.position - transform.position;
        transform.position += Speed * Time.deltaTime * direction.normalized;
        if (Vector3.Distance(transform.position, NextTile.transform.position) < 0.1f) {
            PathNode node = PathFinder.Instance.GetNextNode(NextTile);
            if (node == null) {
                Debug.Log("Reached destination");
                OnReachedDestination.Invoke();
                return;
            }
            NextTile = node.Tile;
        }
    }
}
