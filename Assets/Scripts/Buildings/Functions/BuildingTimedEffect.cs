using UnityEngine;

[RequireComponent(typeof(Timer))]
public abstract class BuildingTimedEffect : BuildingEffect {
    
    [Header("Attributes")]
    [field: SerializeField] public virtual float Duration { get; private set; } = 1f;

    protected Timer timer;

    public void SetDuration(float duration) {
        Duration = duration;
        StartEffect();
    }

    public override void Reapply(BuildingEffect otherEffect) {
        var otherTimedEffect = (BuildingTimedEffect)otherEffect;
        Duration = otherTimedEffect.Duration;
        StartEffect();
    }

    protected void StartEffect() {
        timer.StartTimer(Duration);
    }

    protected virtual void Start() {
        timer.OnDone.AddListener(Remove);
        StartEffect();
    }

    protected virtual void Awake() {
        timer = GetComponent<Timer>();
    }

}
