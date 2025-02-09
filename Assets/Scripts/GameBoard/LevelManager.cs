using UnityEngine;
using System.Collections.Generic;

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
    [field:SerializeField] public GameObject TileParent { get; private set; }
    [field:SerializeField] public GameObject BuildingParent { get; private set; }
    [field:SerializeField] public GameObject EnemyParent { get; private set; }
    [field:SerializeField] public GameObject ProjectileParent { get; private set; }

    [Header("Attributes")]
    public List<Wave> Waves = new();

    public static LevelManager Instance { get; private set; }

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
        StartWave(Waves[0]);
    }

    public void StartWave(Wave wave) {
        foreach (var spawnInfo in wave.SpawnInfos) {
            if (spawnInfo.SpawnerTile == null) {
                Debug.LogError("SpawnerTile is null");
                return;
            }
            spawnInfo.SpawnerTile.StartSpawning(spawnInfo, EnemyParent);
        }
    }
}
