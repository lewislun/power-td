using UnityEngine;

public class ScaleEventHook : MonoBehaviour {

    public void SetScale(float scale) {
        transform.localScale = new Vector3(scale, scale, 1);
    }

    public void SetScaleByTileRange(float range) {
        float desiredScale = range * 2 + 1;
        Vector3 parentLossyScale = transform.parent != null ? transform.parent.lossyScale : Vector3.one;
        float scale = desiredScale / parentLossyScale.x;
        SetScale(scale);
    }

}