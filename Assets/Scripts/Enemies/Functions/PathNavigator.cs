using UnityEngine;
using UnityEngine.Events;

public class PathNavigator : MonoBehaviour {

    [Header("Attributes")]
    public float Speed = 1f;
    public float RandomOffset = 0.05f;

    [Header("Events")]
    public UnityEvent OnReachedDestination;

    [Header("Info")]
    [field:SerializeField, ReadOnly] public Vector2 Offset { get; private set; } = Vector2.zero;

    public ITile NextTile { get; private set; }
    public Vector3 NextPos { get; private set; }

    public void SetNextTile(ITile tile) {
        NextTile = tile;
        NextPos = new Vector3(
            NextTile.transform.position.x + Offset.x,
            NextTile.transform.position.y + Offset.y,
            transform.parent.position.z
        );
    }

    protected void Awake() {
        Offset = new Vector2(Random.Range(-RandomOffset, RandomOffset), Random.Range(-RandomOffset, RandomOffset));
    }

    protected void Update() {
        MoveTowardsNextTile();
    }

    protected void MoveTowardsNextTile() {
        if (NextTile == null) {
            return;
        }

        Vector3 direction = NextPos - transform.position;
        transform.position += Speed * Time.deltaTime * direction.normalized;
        if (Vector3.Distance(transform.position, NextPos) < 0.05f) {
            PathNode node = PathFinder.Instance.GetNextNode(NextTile);
            if (node == null) {
                OnReachedDestination.Invoke();
                return;
            }
            SetNextTile(node.Tile);
        }
    }
}
