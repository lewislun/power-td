using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using System;

public class PathNode: ICloneable {
    public ITile Tile;
    public int x, y, nextX, nextY, destX, destY;

    public PathNode(ITile tile, int x, int y) {
        Tile = tile;
        this.x = x;
        this.y = y;
        nextX = -1;
        nextY = -1;
        destX = -1;
        destY = -1;
    }

    public PathNode Next {
        get {
            if (nextX == -1 || nextY == -1) {
                return null;
            }
            return PathFinder.Instance.Nodes[nextX, nextY];
        }
        set {
            if (value == null) {
                nextX = -1;
                nextY = -1;
            } else {
                nextX = value.x;
                nextY = value.y;
            }
        }
    }

    public PathNode Destination {
        get {
            if (destX == -1 || destY == -1) {
                return null;
            }
            return PathFinder.Instance.Nodes[destX, destY];
        }
        set {
            if (value == null) {
                destX = -1;
                destY = -1;
            } else {
                destX = value.x;
                destY = value.y;
            }
        }
    }

    public object Clone() {
        return new PathNode(Tile, x, y) {
            nextX = nextX,
            nextY = nextY,
            destX = destX,
            destY = destY,
        };
    }
}

public class PathFinder : MonoBehaviour {

    [Header("References")]
    public GameObject TileParent;

    [Header("Events")]
    public UnityEvent OnPathUpdate = new();

    public static PathFinder Instance { get; private set; }

    public PathNode[,] Nodes { get; private set; }

    private readonly Dictionary<ITile, PathNode> nodeByTile = new();
    private readonly List<ITile> spawnerTiles = new();
    private readonly List<ITile> destinationTiles = new();
    private PathNode[,] NodesStash;

    public PathNode GetNextNode(ITile tile) {
        return nodeByTile[tile].Next;
    }

    // Check if there is a valid path for all spawners
    public bool AreDestinationsReachableFromAllSpawners() {
        foreach (ITile tile in spawnerTiles) {
            if (nodeByTile[tile].Destination == null) {
                return false;
            }
        }
        return true;
    }

    public List<PathNode> GetPath(ITile tile) {
        List<PathNode> path = new();
        PathNode node = nodeByTile[tile] ?? throw new System.Exception("Tile does not exist in pathfinder");
        if (node.Destination == null) {
            return path;
        }

        while (node != null) {
            path.Add(node);
            node = node.Next;
        }
        return path;
    }

    // Try to update paths and return true if all destinations are reachable from all spawners
    public bool TryUpdatePaths() {
        StashNodes();
        UpdatePaths(false);
        if (AreDestinationsReachableFromAllSpawners()) {
            OnPathUpdate.Invoke();
            return true;
        }
        RestoreNodes();
        return false;
    }

    public void UpdateNodes() {
        ITile[] allTiles = TileParent.GetComponentsInChildren<ITile>();
        Debug.Log(allTiles.Length + " tiles found");
        HashSet<float> xSet = new();
        HashSet<float> ySet = new();
        foreach (ITile tile in allTiles) {
            xSet.Add(tile.transform.position.x);
            ySet.Add(tile.transform.position.y);
        }
        float[] xArray = new float[xSet.Count];
        float[] yArray = new float[ySet.Count];
        xSet.CopyTo(xArray);
        ySet.CopyTo(yArray);
        Array.Sort(xArray);
        Array.Sort(yArray);

        Nodes = new PathNode[xArray.Length, yArray.Length];
        spawnerTiles.Clear();
        destinationTiles.Clear();
        nodeByTile.Clear();
        foreach (ITile tile in allTiles) {
            int xIndex = Array.IndexOf(xArray, tile.transform.position.x);
            int yIndex = Array.IndexOf(yArray, tile.transform.position.y);
            if (Nodes[xIndex, yIndex] != null) {
                Debug.LogError("Duplicated tile at " + tile.transform.position);
                continue;
            }
            Nodes[xIndex, yIndex] = new PathNode(tile, xIndex, yIndex);
            nodeByTile[tile] = Nodes[xIndex, yIndex];

            if (tile is SpawnerTile) {
                spawnerTiles.Add(tile);
            } else if (tile is DestinationTile) {
                destinationTiles.Add(tile);
                Nodes[xIndex, yIndex].Destination = Nodes[xIndex, yIndex];
            }
        }
    }

    public void UpdatePaths(bool InvokeOnPathUpdate = true) {
        ClearPaths();
        Queue<PathNode> queue = new();
        HashSet<PathNode> visitedSet = new();
        foreach (var tile in destinationTiles) {
            queue.Enqueue(nodeByTile[tile]);
            visitedSet.Add(nodeByTile[tile]);
        }
        
        int[][] directions = new int[][] {
            new int[2] {0, 1},
            new int[2] {0, -1},
            new int[2] {1, 0},
            new int[2] {-1, 0},
            new int[2] {1, 1},
            new int[2] {1, -1},
            new int[2] {-1, 1},
            new int[2] {-1, -1},
        };
        while (queue.Count > 0) {
            PathNode node = queue.Dequeue();
            if (node.Tile.IsPassable == false) {
                continue;
            }
            foreach (int[] direction in directions) {
                int x = node.x + direction[0];
                int y = node.y + direction[1];
                if (x < 0 || x >= Nodes.GetLength(0) || y < 0 || y >= Nodes.GetLength(1)) {
                    continue;
                }
                // check corners for diagonal movement
                if (direction[0] != 0 && direction[1] != 0) {
                    if (Nodes[x, node.y] == null || Nodes[node.x, y] == null || Nodes[x, node.y].Tile.IsPassable == false || Nodes[node.x, y].Tile.IsPassable == false) {
                        continue;
                    }
                }

                PathNode nextNode = Nodes[x, y];
                if (nextNode == null || visitedSet.Contains(nextNode)) {
                    continue;
                }
                nextNode.Destination = node.Destination;
                nextNode.Next = node;
                queue.Enqueue(nextNode);
                visitedSet.Add(nextNode);
            }
        }

        if (InvokeOnPathUpdate) {
            OnPathUpdate.Invoke();
        }
    }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Debug.LogError("Multiple PathFinder instances");
        }
        if (TileParent == null) {
            TileParent = GameObject.Find("Tiles");
        }
    }

    private void Start() {
        UpdateNodes();
        UpdatePaths();
    }

    private void ClearPaths() {
        foreach (PathNode node in nodeByTile.Values) {
            if (node.Tile is not DestinationTile) {
                node.Destination = null;
            }
            node.Next = null;
        }
    }

    private void StashNodes() {
        NodesStash = new PathNode[Nodes.GetLength(0), Nodes.GetLength(1)];
        for (int x = 0; x < Nodes.GetLength(0); x++) {
            for (int y = 0; y < Nodes.GetLength(1); y++) {
                if (Nodes[x, y] == null) {
                    continue;
                }
                NodesStash[x, y] = (PathNode)Nodes[x, y].Clone();
            }
        }
    }

    private void RestoreNodes() {
        if (NodesStash == null) {
            Debug.Log("NodesStash is null");
            return;
        }
        nodeByTile.Clear();
        for (int x = 0; x < NodesStash.GetLength(0); x++) {
            for (int y = 0; y < NodesStash.GetLength(1); y++) {
                if (NodesStash[x, y] == null) {
                    continue;
                }
                Nodes[x, y] = NodesStash[x, y];
                nodeByTile[Nodes[x, y].Tile] = Nodes[x, y];
            }
        }
        NodesStash = null;
    }
}
