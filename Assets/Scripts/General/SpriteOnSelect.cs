using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteOnSelect : MonoBehaviour {

    [Header("References")]
    [SerializeField] public GameObject SelectableObj;

    protected SpriteRenderer spriteRenderer;

    protected void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
        if (SelectableObj == null) {
            Debug.LogError("SelectableObj is not set in " + name);
        }
    }

    protected void Start() {
        SelectionManager.Instance.RegisterOnSelect(SelectableObj, OnSelect);
        SelectionManager.Instance.RegisterOnDeselect(SelectableObj, OnDeselect);
    }

    protected void OnSelect() {
        spriteRenderer.enabled = true;
    }

    protected void OnDeselect() {
        spriteRenderer.enabled = false;
    }
}
