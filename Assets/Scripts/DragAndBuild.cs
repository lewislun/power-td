using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndBuild : MonoBehaviour {

    [Header("References")]
    public GameObject BuildingPrefab;
    public GameObject BuildingParent;

    private GameObject currentBuilding;

    static private Vector3 PointerPosToWorldPos(PointerEventData pointerEventData) {
        Vector3 pos = Camera.main.ScreenToWorldPoint(pointerEventData.position);
        pos.z = 0;
        return pos;
    }

    private void Awake() {
        if (BuildingParent == null) {
            BuildingParent = GameObject.Find("Buildings");
        }
    }

    public void BeginDragHandler(BaseEventData eventData) {
        if (BuildingPrefab == null) {
            Debug.LogError("BuildingPrefab is null");
            return;
        }

        Vector3 position = PointerPosToWorldPos((PointerEventData)eventData);
        currentBuilding = Instantiate(BuildingPrefab, position, Quaternion.identity, BuildingParent.transform);
        Pausable.Pause(currentBuilding);
    }

    public void DragHandler(BaseEventData eventData) {
        if (currentBuilding == null) {
            return;
        }

        Vector3 position = PointerPosToWorldPos((PointerEventData)eventData);
        currentBuilding.transform.position = position;
    }

    public void EndDragHandler(BaseEventData eventData) {
        if (currentBuilding == null) {
            return;
        }
        Pausable.Unpause(currentBuilding);
        currentBuilding = null;
        // TODO: assign building to tile
        // TODO: check currency
    }
}
