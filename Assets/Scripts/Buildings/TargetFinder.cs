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
    [SerializeField]
    private float _range = 1;
    public float Range {
        get => _range;
        set {
            _range = value;
            float scale = transform.localScale.x;
            GetComponent<CircleCollider2D>().radius = (_range + 0.5f) / scale;
        }
    }
    public TargetPriority targetPriority = TargetPriority.Closest;
    public TargetSwitchStrategy targetSwitchStrategy = TargetSwitchStrategy.UntilLost;

    [Header("Events")]
    public UnityEvent OnTargetChanged;

    [Header("Information")]
    [field:SerializeField] public Transform CurrentTarget { get; private set; }


    private HashSet<Transform> availableTargets = new();
    private readonly Type targetType = typeof(Enemy);

    void OnValidate() {
        // Trigger setters when the value is changed in the editor
        Range = _range;
    }

    private void OnTriggerEnter2D(Collider2D trigger) {
        Component component = trigger.GetComponent(targetType);
        if (component) {
            availableTargets.Add(component.transform);
            if (CurrentTarget == null) {
                SwitchTarget();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D trigger) {
        Component component = trigger.GetComponent(targetType);
        if (component) {
            availableTargets.Remove(component.transform);
            if (CurrentTarget == component.transform) {
                SwitchTarget();
            }
        }
    }

    private void SwitchTarget() {
        Transform prevTarget = CurrentTarget;
        CurrentTarget = targetPriority switch {
            TargetPriority.Closest => GetClosestTarget(),
            TargetPriority.Furthest => GetFurthestTarget(),
            TargetPriority.Random => GetRandomTarget(),
            _ => GetClosestTarget(),
        };
        if (prevTarget != CurrentTarget) {
            OnTargetChanged.Invoke();
        }
    }

    private Transform GetClosestTarget() {
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

    private Transform GetFurthestTarget() {
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

    private Transform GetRandomTarget() {
        int randomIndex = UnityEngine.Random.Range(0, availableTargets.Count);
        return new List<Transform>(availableTargets)[randomIndex];
    }

    private void Update() {
        if (targetSwitchStrategy == TargetSwitchStrategy.FocusPriority) {
            SwitchTarget();
        }
    }
}
