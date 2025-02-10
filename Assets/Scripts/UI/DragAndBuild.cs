using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndBuild : MonoBehaviour {

    [Header("References")]
    public GameObject BuildingPrefab;

    private Building currentBuilding;

    static private Vector3 PointerPosToWorldPos(PointerEventData pointerEventData) {
        Vector3 pos = Camera.main.ScreenToWorldPoint(pointerEventData.position);
        pos.z = LevelManager.Instance.BuildingParent.transform.position.z;
        return pos;
    }

    static private BuildableTile GetBuildableTile(Vector3 position) {
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero, 10f, LayerMask.GetMask(Layer.Tile));
        if (hit.collider != null && hit.collider.TryGetComponent<BuildableTile>(out var tile)) {
            return tile;
        }
        return null;
    }

    public void BeginDragHandler(BaseEventData eventData) {
        if (BuildingPrefab == null || BuildingPrefab.GetComponent<Building>() == null) {
            Debug.LogError("BuildingPrefab is null or is not a Building");
            return;
        }

        Vector3 position = PointerPosToWorldPos((PointerEventData)eventData);
        Transform parent = LevelManager.Instance.BuildingParent.transform;
        GameObject building = Instantiate(BuildingPrefab, position, Quaternion.identity, parent);
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
