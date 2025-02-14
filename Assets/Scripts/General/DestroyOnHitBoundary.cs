using UnityEngine;

public class DestroyOnHitBoundary : MonoBehaviour {
    protected void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer(Layer.Boundary)) {
            Destroy(gameObject);
        }
    }
}