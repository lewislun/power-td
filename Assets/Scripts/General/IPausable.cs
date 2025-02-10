using UnityEngine;

public interface IPausable {
    public bool IsPaused { get; }
    public void Pause();
    public void Unpause();
}

public static class Pausable {
    public static void Pause(GameObject gameObject) {
        foreach (IPausable pausable in gameObject.GetComponentsInChildren<IPausable>()) {
            pausable.Pause();
        }
    }

    public static void Unpause(GameObject gameObject) {
        foreach (IPausable pausable in gameObject.GetComponentsInChildren<IPausable>()) {
            pausable.Unpause();
        }
    }
}