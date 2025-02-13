using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.Events;

public enum TargetPriority {
    Closest,
    Furthest,
    Random,
    // LowestHealth,
    // HighestHealth,
}

public enum TargetSwitchStrategy {
    UntilLost,
    FocusPriority,
}

[RequireComponent(typeof(CircleCollider2D))]
public class TargetFinder : MonoBehaviour {

    [Header("Attributes")]
    public TargetPriority TargetPriority = TargetPriority.Closest;
    public TargetSwitchStrategy TargetSwitchStrategy = TargetSwitchStrategy.UntilLost;

    [Header("Events")]
    public UnityEvent OnTargetChanged;

    [Header("Info")]
    [field:SerializeField, ReadOnly] public float Range { get; private set; } = 1f;
    [field:SerializeField, ReadOnly] public Transform CurrentTarget { get; private set; }


    private readonly HashSet<Transform> availableTargets = new();
    private readonly Type targetType = typeof(Enemy);
    private CircleCollider2D circleCollider;


    public void SetRange(float range) {
        Range = range;
        UpdateColliderRadius();
    }

    public void UpdateColliderRadius() {
        circleCollider.radius = (Range + 0.5f) / transform.lossyScale.x;
    }

    protected void Awake() {
        circleCollider = GetComponent<CircleCollider2D>();
    }

    protected void Update() {
        if (TargetSwitchStrategy == TargetSwitchStrategy.FocusPriority) {
            SwitchTarget();
        }
    }

    protected void OnTriggerEnter2D(Collider2D trigger) {
        Component component = trigger.GetComponent(targetType);
        if (component) {
            availableTargets.Add(component.transform);
            if (CurrentTarget == null) {
                SwitchTarget();
            }
        }
    }

    protected void OnTriggerExit2D(Collider2D trigger) {
        Component component = trigger.GetComponent(targetType);
        if (component) {
            availableTargets.Remove(component.transform);
            if (CurrentTarget == component.transform) {
                SwitchTarget();
            }
        }
    }

    protected void SwitchTarget() {
        Transform prevTarget = CurrentTarget;
        CurrentTarget = TargetPriority switch {
            TargetPriority.Closest => GetClosestTarget(),
            TargetPriority.Furthest => GetFurthestTarget(),
            TargetPriority.Random => GetRandomTarget(),
            _ => GetClosestTarget(),
        };
        if (prevTarget != CurrentTarget) {
            OnTargetChanged.Invoke();
        }
    }

    protected Transform GetClosestTarget() {
        Transform closestTarget = null;
        float closestDistance = float.MaxValue;
        foreach (Transform target in availableTargets) {
            if (closestTarget == null) {
                closestTarget = target;
                continue;
            }

            if (Vector2.Distance(transform.position, target.position) < closestDistance) {
                closestTarget = target;
            }
        }

        return closestTarget;
    }

    protected Transform GetFurthestTarget() {
        Transform furthestTarget = null;
        float furthestDistance = float.MinValue;
        foreach (Transform target in availableTargets) {
            if (furthestTarget == null) {
                furthestTarget = target;
                continue;
            }

            if (Vector2.Distance(transform.position, target.position) > furthestDistance) {
                furthestTarget = target;
            }
        }

        return furthestTarget;
    }

    protected Transform GetRandomTarget() {
        int randomIndex = UnityEngine.Random.Range(0, availableTargets.Count);
        return new List<Transform>(availableTargets)[randomIndex];
    }
}
