using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SelectionManager : MonoBehaviour {

    [Header("Info")]
    [field:SerializeField, ReadOnly] public GameObject SelectedObject { get; private set; }

    public static SelectionManager Instance { get; private set; }

    protected Dictionary<GameObject, UnityEvent> OnSelectByObj { get; private set; } = new();
    protected Dictionary<GameObject, UnityEvent> OnDeselectByObj { get; private set; } = new();

    public bool ToggleSelect(GameObject obj) {
        if (SelectedObject == obj) {
            Deselect();
            return false;
        } else {
            return Select(obj);
        }
    }

    public bool Select(GameObject obj) {
        Deselect();
        SelectedObject = obj;
        if (OnSelectByObj.ContainsKey(SelectedObject)) {
            OnSelectByObj[SelectedObject].Invoke();
        }
        return true;
    }

    public bool Deselect() {
        if (SelectedObject == null) {
            return false;
        }
        if (OnDeselectByObj.ContainsKey(SelectedObject)) {
            OnDeselectByObj[SelectedObject].Invoke();
        }
        
        return true;
    }

    public void RegisterOnSelect(GameObject obj, UnityAction action) {
        if (!OnSelectByObj.ContainsKey(obj)) {
            OnSelectByObj[obj] = new UnityEvent();
        }
        OnSelectByObj[obj].AddListener(action);
    }

    public void RegisterOnDeselect(GameObject obj, UnityAction action) {
        if (!OnDeselectByObj.ContainsKey(obj)) {
            OnDeselectByObj[obj] = new UnityEvent();
        }
        OnDeselectByObj[obj].AddListener(action);
    }

    protected void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Debug.LogError("Multiple SelectionManagers in scene");
        }
    }
}
