using System;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour {

    [Header("Attributes")]
    [field:SerializeField] public float Time { get; private set; } = 1f;

    [Header("Events")]
    [field: SerializeField] public UnityEvent OnStart { get; private set; } = new();
    [field: SerializeField] public UnityEvent OnDone { get; private set; } = new();
    [field: SerializeField] public UnityEvent<float, float> OnTick { get; private set; } = new();

    [Header("Info")]
    [field: SerializeField, ReadOnly] public float CurrentTime { get; private set; } = 0f;
    [field: SerializeField, ReadOnly] public bool IsDone { get; private set; } = true;

    public void StartTimer() {
        CurrentTime = 0f;
        IsDone = false;
        OnStart.Invoke();
    }

    public void StartTimer(float Time) {
        this.Time = Time;
        StartTimer();
    }

    protected void Update() {
        if (!IsDone) {
            CurrentTime = Math.Clamp(CurrentTime + UnityEngine.Time.deltaTime, 0, Time);
            OnTick.Invoke(CurrentTime, Time);
            if (CurrentTime >= Time) {
                OnDone.Invoke();
                IsDone = true;
            }
        }
    }

}