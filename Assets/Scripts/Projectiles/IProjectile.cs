using UnityEngine;

public interface IProjectile {
    public float TravelSpeed { get; set; }
    public float Damage { get; set; }
    public Transform Target { get; set; }
    public void DestroyProjectile();
}