using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SpawnerTile : MonoBehaviour, ITile {

    [Header("References")]
    [field:SerializeField] public LineRenderer PathLineRenderer { get; private set; }

    [Header("Debug")]
    public bool IsSpawning => spawnRoutine != null;

    public bool IsPassable { get => true; }
    public bool IsBuildable { get => false; }

    private Coroutine spawnRoutine;

    public void StartSpawning(SpawnInfo spawnInfo, GameObject parent) {
        if (IsSpawning) {
            Debug.LogError("Already spawning");
            return;
        }

        spawnRoutine = StartCoroutine(SpawnAll(spawnInfo, parent));
    }

    public void StopSpawning() {
        if (spawnRoutine != null) {
            StopCoroutine(spawnRoutine);
            spawnRoutine = null;
        }
    }

    protected void Awake() {
        if (PathLineRenderer == null) {
            Debug.LogError("PathLineRenderer is null");
        }
        PathFinder.Instance.OnPathUpdate.AddListener(UpdatePathDisplay);
    }

    protected void Start(){
    }

    protected IEnumerator SpawnAll(SpawnInfo spawnInfo, GameObject parent) {
        Debug.Log($"Start spawning from {spawnInfo.SpawnerTile.name}");

        yield return new WaitForSeconds(spawnInfo.SecBeforeStart);
        foreach (EnemyBatch batch in spawnInfo.Batches) {
            yield return new WaitForSeconds(batch.SecBeforeSpawns);
            for (int i = 0; i < batch.Count; i++) {
                Spawn(batch.Prefab, parent);
                if (i < batch.Count - 1) {
                    yield return new WaitForSeconds(batch.SecBetweenSpawns);
                }
            }
        }
        spawnRoutine = null;

        Debug.Log($"Finished spawning from {spawnInfo.SpawnerTile.name}");
    }

    protected GameObject Spawn(GameObject prefab, GameObject parent) {
        Vector3 pos = new(transform.position.x, transform.position.y, parent.transform.position.z);
        GameObject enemy = Instantiate(prefab, pos, Quaternion.identity, parent.transform);
        enemy.GetComponent<PathNavigator>().SetNextTile(this);
        return enemy;
    }

    protected void UpdatePathDisplay() {
        List<PathNode> nodes = PathFinder.Instance.GetPath(this);
        Vector3[] positions = new Vector3[nodes.Count];
        for (int i = 0; i < nodes.Count; i++) {
            positions[i] = nodes[i].Tile.transform.position;
        }
        PathLineRenderer.positionCount = positions.Length;
        PathLineRenderer.SetPositions(positions);
    }
}
