using UnityEngine;
using UnityEngine.Events;

public class PathNavigator : MonoBehaviour {

    [Header("Attributes")]
    public float Speed = 1f;
    public float RandomOffset = 0.05f;

    [Header("Events")]
    public UnityEvent OnReachedDestination;

    [Header("Debug")]
    [ReadOnly] public ITile NextTile;
    [ReadOnly] public Vector3 NextPos;

    public void SetNextTile(ITile tile) {
        NextTile = tile;
        NextPos = new Vector3(
            NextTile.transform.position.x + Random.Range(-RandomOffset, RandomOffset),
            NextTile.transform.position.y + Random.Range(-RandomOffset, RandomOffset),
            0
        );
    }

    private void Update() {
        MoveTowardsNextTile();
    }

    private void MoveTowardsNextTile() {
        if (NextTile == null) {
            return;
        }

        Vector3 direction = NextPos - transform.position;
        transform.position += Speed * Time.deltaTime * direction.normalized;
        if (Vector3.Distance(transform.position, NextPos) < 0.05f) {
            PathNode node = PathFinder.Instance.GetNextNode(NextTile);
            if (node == null) {
                Debug.Log("Reached destination");
                OnReachedDestination.Invoke();
                return;
            }
            SetNextTile(node.Tile);
        }
    }
}
