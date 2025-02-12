using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class Selectable: MonoBehaviour, IPointerClickHandler {

    [Header("Events")]
    public UnityEvent OnSelected = new();  // invoked by SelectionManager
    public UnityEvent OnDeselected = new();  // invoked by SelectionManager

    public void OnPointerClick(PointerEventData eventData) {
        SelectionManager.Instance.ToggleSelect(this);
    }
}