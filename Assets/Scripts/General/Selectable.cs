using UnityEngine;
using UnityEngine.EventSystems;

public class Selectable: MonoBehaviour, IPointerClickHandler {
    public void OnPointerClick(PointerEventData eventData) {
        SelectionManager.Instance.ToggleSelect(gameObject);
    }
}