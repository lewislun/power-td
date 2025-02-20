using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

[System.Serializable]
public class Wave {
    public List<SpawnInfo> SpawnInfos = new();
}

[System.Serializable]
public class SpawnInfo {
    public SpawnerTile SpawnerTile;
    public List<EnemyBatch> Batches;
    public float SecBeforeStart;
}

[System.Serializable]
public class EnemyBatch {
    public GameObject Prefab;
    public int Count;
    public float SecBeforeSpawns;
    public float SecBetweenSpawns;
}

public class LevelManager : MonoBehaviour {

    [Header("References")]
    [field: SerializeField] public GameObject TileParent { get; private set; }
    [field: SerializeField] public GameObject BuildingParent { get; private set; }
    [field: SerializeField] public GameObject EnemyParent { get; private set; }
    [field: SerializeField] public GameObject ProjectileParent { get; private set; }

    [Header("Attributes")]
    public List<Wave> Waves = new();

    [Header("Events")]
    [field: SerializeField] public UnityEvent OnWaveStart { get; private set; } = new();
    [field: SerializeField] public UnityEvent OnWaveEnd { get; private set; } = new();

    [Header("Info")]
    [field: SerializeField] public bool IsWaveActive { get; private set; } = false;
    [field: SerializeField] public int CurrentWaveIndex { get; private set; } = -1;
    [field: SerializeField] public HashSet<Enemy> CurrentEnemySet { get; private set; } = new();
    [field: SerializeField] public HashSet<SpawnerTile> CurrentSpawningSpawnerSet { get; private set; } = new();

    public static LevelManager Instance { get; private set; }

    public void StartNextWave() {
        if (CurrentWaveIndex + 1 >= Waves.Count) {
            Debug.LogError("No more waves");
            return;
        }
        StartWave(CurrentWaveIndex + 1);
    }

    public void StartWave(int waveIndex) {
        if (waveIndex < 0 || waveIndex >= Waves.Count) {
            Debug.LogError("Invalid wave index");
            return;
        }
        CurrentWaveIndex = waveIndex;
        StartWave(Waves[waveIndex]);
    }

    public void StartWave(Wave wave) {
        IsWaveActive = true;
        OnWaveStart.Invoke();
        // Register all spawners first to avoid invoking OnWaveEnd before all spawners are done spawning
        foreach (var spawnInfo in wave.SpawnInfos) {
            if (spawnInfo.SpawnerTile == null) {
                Debug.LogError("SpawnerTile is null");
                return;
            }
            CurrentSpawningSpawnerSet.Add(spawnInfo.SpawnerTile);
        }
        foreach (var spawnInfo in wave.SpawnInfos) {
            spawnInfo.SpawnerTile.StartSpawning(spawnInfo, EnemyParent);
        }
    }

    public void UnregisterSpawningSpawner(SpawnerTile spawnerTile) {
        CurrentSpawningSpawnerSet.Remove(spawnerTile);
        if (CurrentSpawningSpawnerSet.Count == 0) {
            CheckWaveEnd();
        }
    }

    public void RegisterEnemy(Enemy enemy) {
        CurrentEnemySet.Add(enemy);
    }

    public void UnregisterEnemy(Enemy enemy) {
        CurrentEnemySet.Remove(enemy);
        if (CurrentEnemySet.Count == 0) {
            CheckWaveEnd();
        }
    }

    private void CheckWaveEnd() {
        if (IsWaveActive && CurrentEnemySet.Count == 0 && CurrentSpawningSpawnerSet.Count == 0) {
            IsWaveActive = false;
            OnWaveEnd.Invoke();
        }
    }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Debug.LogError("Multiple LevelManager instances");
        }

        if (TileParent == null) {
            Debug.LogError("TileParent is null");
        }
        if (EnemyParent == null) {
            Debug.LogError("EnemyParent is null");
        }
        if (BuildingParent == null) {
            Debug.LogError("BuildingParent is null");
        }
        if (ProjectileParent == null) {
            Debug.LogError("ProjectileParent is null");
        }
    }

    private void Start() {
        if (Waves.Count == 0) {
            Debug.LogError("No waves");
            return;
        }
    }
}
