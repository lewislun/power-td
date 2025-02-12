using UnityEngine;

public class SelectionManager : MonoBehaviour {

    [Header("Info")]
    [field:SerializeField, ReadOnly] public Selectable SelectedItem { get; private set; }

    public static SelectionManager Instance { get; private set; }

    public bool ToggleSelect(Selectable selectable) {
        if (SelectedItem == selectable) {
            Deselect();
            return false;
        } else {
            Select(selectable);
            return true;
        }
    }

    public void Select(Selectable selectable) {
        Deselect();
        SelectedItem = selectable;
        selectable.OnSelected.Invoke();
    }

    public void Deselect() {
        if (SelectedItem == null) {
            return;
        }
        SelectedItem.OnDeselected.Invoke();
        SelectedItem = null;
    }

    protected void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Debug.LogError("Multiple SelectionManagers in scene");
        }
    }
}
