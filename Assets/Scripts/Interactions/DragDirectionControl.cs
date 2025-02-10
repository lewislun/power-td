using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class DragDirectionControl : MonoBehaviour, IDragHandler {

    [Header("Attributes")]
    public Transform Anchor;

    [Header("Events")]
    public UnityEvent<Vector2> OnDirectionChange;

    [Header("Information")]
    [field: SerializeField, ReadOnly] public Vector2 Direction { get; private set; } = Vector2.zero;

    public void OnDrag(PointerEventData eventData) {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        Direction = (worldPos - (Vector2)Anchor.position).normalized;
        OnDirectionChange.Invoke(Direction);
    }

    protected void Awake() {
        if (Anchor == null) {
            Debug.LogError("Anchor is null");
        }
    }
}
