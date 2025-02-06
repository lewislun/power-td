using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public enum TargetPriority {
    Closest,
    Furthest,
    Random,
    First,
    Last
}

public enum TargetSwitchStrategy {
    UntilDeath,
    FocusPriority,
}

[RequireComponent(typeof(CircleCollider2D))]
public class TargetFinder : MonoBehaviour
{
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
    public TargetSwitchStrategy targetSwitchStrategy = TargetSwitchStrategy.UntilDeath;

    [Header("Information")]
    [SerializeField] private Transform currentTarget;

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
            // TODO: switch target
        }
    }

    private void OnTriggerExit2D(Collider2D trigger) {
        Component component = trigger.GetComponent(targetType);
        if (component) {
            availableTargets.Remove(component.transform);
            // TODO: switch target
        }
    }

    private void UpdateCurrentTarget() {

    }

    private void Start()
    {
        // Debug.Log("TargetFinder started");
    }
}
