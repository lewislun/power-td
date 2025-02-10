using UnityEngine;

public class RotateAroundAnchor : MonoBehaviour {

  	[Header("Attributes")]
    public Transform Anchor;
    public float Radius = 0f;  // if set to 0, will use the distance from the anchor to the object
    [field:SerializeField] public Vector2 Direction { get; private set; } = Vector2.up;

    public void SetDirection(Vector2 direction) {
        Direction = direction;
        UpdatePosition();
        UpdateRotation();
    }

    protected void Awake() {
        if (Anchor == null) {
            Debug.LogError("Anchor is null");
        } else if (Radius == 0) {
            Radius = Vector2.Distance((Vector2)Anchor.position, (Vector2)transform.position);
        }
    }

    protected void UpdatePosition() {
        float prevZ = transform.position.z;
        Vector3 newPos = Anchor.position + (Vector3)Direction * Radius;
        newPos.z = prevZ;
        transform.position = newPos;
    }

    protected void UpdateRotation() {
        float angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
