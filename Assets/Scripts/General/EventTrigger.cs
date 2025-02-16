using UnityEngine;
using UnityEngine.Events;

// A general class to reference prefabs and trigger events
public class EventTrigger : MonoBehaviour {

    [field: SerializeField] public UnityEvent OnTrigger { get; private set; } = new();

    public void Trigger() {
        OnTrigger.Invoke();
    }

}
