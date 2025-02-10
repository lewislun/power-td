using UnityEngine;
using UnityEngine.EventSystems;

public interface ISelectable: IPointerClickHandler {
    public new void OnPointerClick(PointerEventData eventData) {
        Debug.Log("ISelectable.OnPointerClick");
    }
}