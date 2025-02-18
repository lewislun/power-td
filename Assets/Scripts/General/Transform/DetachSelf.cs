using UnityEngine;

public class DetachSelf : MonoBehaviour {

    public void Detach() {
        transform.parent = transform.parent.parent;
    }

}