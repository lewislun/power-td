using UnityEngine;

public class FaceTarget : MonoBehaviour {

  	[Header("Info")]
    [field: SerializeField] public Transform Target { get; private set; }

    public void SetTarget(Transform target) {
        Target = target;
    }

    protected void Update() {
        if (Target == null) {
            return;
        }
        // Rotate z axis to face target
        Vector3 direction = Target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
