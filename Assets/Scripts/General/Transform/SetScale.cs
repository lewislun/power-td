using UnityEngine;

public class SetScale : MonoBehaviour {

    public void SetLocalScale(float scale) {
        transform.localScale = new Vector3(scale, scale, scale);
    }

}