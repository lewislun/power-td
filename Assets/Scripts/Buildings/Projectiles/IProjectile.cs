using UnityEngine;
using UnityEngine.Events;

public interface IProjectile {
    public float TravelSpeed { get; set; }
    public float Damage { get; set; }
    public Transform Target { get; }
    public UnityEvent<Transform> OnTargetChanged { get; }
    public UnityEvent<Transform> OnTargetHit { get; }
    public void SetTarget(Transform target);
    public void DestroyProjectile(bool shouldPlayEffect);
}