using UnityEngine;

public class CounterParentRotation : MonoBehaviour {

  	private Vector3 initialLocalPos;

    private void Awake() {
        initialLocalPos = transform.localPosition;
    }

    private void Update() {
        transform.rotation = Quaternion.Euler(Vector3.zero);
        transform.localPosition = Quaternion.Inverse(transform.parent.rotation) * initialLocalPos;
    }
}
