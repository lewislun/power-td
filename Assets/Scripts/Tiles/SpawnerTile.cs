using System.Collections;
using UnityEngine;

public class SpawnerTile : MonoBehaviour, ITile {

    public bool IsPassable { get => true; }
    public bool IsBuildable { get => false; }

    [Header("Debug")]
    public bool IsSpawning => spawnRoutine != null;

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

    private IEnumerator SpawnAll(SpawnInfo spawnInfo, GameObject parent) {
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

    private GameObject Spawn(GameObject prefab, GameObject parent) {
        Vector3 pos = new(transform.position.x, transform.position.y, parent.transform.position.z);
        GameObject enemy = Instantiate(prefab, pos, Quaternion.identity, parent.transform);
        enemy.GetComponent<PathNavigator>().SetNextTile(this);
        return enemy;
    }
}
