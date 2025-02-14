using System;
using UnityEngine;
using UnityEngine.Events;

public class FaceTarget : MonoBehaviour {

    [Header("Attributes")]
    public float RotationDegPerSec = 360f;
    public float FaceTargetDegThreshold = 5f;

    [Header("Events")]
    [field: SerializeField] public UnityEvent OnFacingTarget { get; private set; } = new();
    [field: SerializeField] public UnityEvent OnNotFacingTarget { get; private set; } = new();

    [Header("Info")]
    [field: SerializeField, ReadOnly] public Transform Target { get; private set; }
    [field: SerializeField, ReadOnly] public bool IsFacingTarget { get; private set; } = false;
    

    public void SetTarget(Transform target) {
        Target = target;
    }

    protected void Update() {
        if (Target == null) {
            if (IsFacingTarget) {
                IsFacingTarget = false;
                OnNotFacingTarget.Invoke();
            }
            return;
        }

        // Rotate z axis to face target
        var relativeAngleDeg = GetRelativeAngleDeg();
        var targetRotation = Quaternion.Euler(0, 0, relativeAngleDeg);
        if (transform.rotation == targetRotation) {
            return;
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, RotationDegPerSec * Time.deltaTime);

        // Invoke events
        var isFacingTarget = Quaternion.Angle(transform.rotation, targetRotation) < FaceTargetDegThreshold;
        if (isFacingTarget && !IsFacingTarget) {
            IsFacingTarget = true;
            OnFacingTarget.Invoke();
        } else if (!isFacingTarget && IsFacingTarget) {
            IsFacingTarget = false;
            OnNotFacingTarget.Invoke();
        }
    }

    protected float GetRelativeAngleDeg() {
        return Mathf.Atan2(Target.position.y - transform.position.y, Target.position.x - transform.position.x) * Mathf.Rad2Deg - 90;
    }
}
