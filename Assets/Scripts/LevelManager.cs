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
    [field:SerializeField] public GameObject EnemyParent { get; set; }

    [Header("Attributes")]
    public List<Wave> Waves = new();

    private void Awake() {
        if (EnemyParent == null) {
            EnemyParent = GameObject.Find("Enemies");
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
