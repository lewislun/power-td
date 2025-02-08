using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndBuild : MonoBehaviour {

    [Header("References")]
    public GameObject BuildingPrefab;
    public GameObject BuildingParent;

    private Building currentBuilding;

    static private Vector3 PointerPosToWorldPos(PointerEventData pointerEventData) {
        Vector3 pos = Camera.main.ScreenToWorldPoint(pointerEventData.position);
        pos.z = 0;
        return pos;
    }

    static private BuildableTile GetBuildableTile(Vector3 position) {
        RaycastHit2D[] hits = Physics2D.RaycastAll(position, Vector2.zero);
        foreach (RaycastHit2D hit in hits) {
            if (hit.collider.TryGetComponent<BuildableTile>(out var tile)) {
                return tile;
            }
        }
        return null;
    }

    private void Awake() {
        if (BuildingParent == null) {
            BuildingParent = GameObject.Find("Buildings");
        }
    }

    public void BeginDragHandler(BaseEventData eventData) {
        if (BuildingPrefab == null || BuildingPrefab.GetComponent<Building>() == null) {
            Debug.LogError("BuildingPrefab is null or is not a Building");
            return;
        }

        Vector3 position = PointerPosToWorldPos((PointerEventData)eventData);
        GameObject building = Instantiate(BuildingPrefab, position, Quaternion.identity, BuildingParent.transform);
        currentBuilding = building.GetComponent<Building>();
    }

    public void DragHandler(BaseEventData eventData) {
        if (currentBuilding == null) {
            return;
        }

        Vector3 position = PointerPosToWorldPos((PointerEventData)eventData);
        BuildableTile tile = GetBuildableTile(position);
        if (tile && currentBuilding.IsBuildableAt(tile)) {
            position = tile.transform.position;
        }

        currentBuilding.transform.position = position;
    }

    public void EndDragHandler(BaseEventData eventData) {
        if (currentBuilding == null) {
            return;
        }

        Vector3 position = PointerPosToWorldPos((PointerEventData)eventData);
        BuildableTile tile = GetBuildableTile(position);
        if (!tile || !currentBuilding.IsBuildableAt(tile) || !currentBuilding.Build(tile)) {
            Debug.Log("Cannot build here");
            Destroy(currentBuilding.gameObject);
        }

        currentBuilding = null;
    }
}
