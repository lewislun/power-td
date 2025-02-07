using UnityEngine;
using System.Collections.Generic;

public class PathNode {
    public ITile Tile;
    public int x, y;
    public PathNode Next;
    public PathNode Destination;
}

public class PathFinder : MonoBehaviour {

    [Header("References")]
    public GameObject TileParent;

    public static PathFinder Instance { get; private set; }

    private PathNode[,] nodes;
    private readonly Dictionary<ITile, PathNode> nodeByTile = new();
    private readonly List<PathNode> spawnerNodes = new();
    private readonly List<PathNode> destinationNodes = new();

    public PathNode GetNextNode(ITile tile) {
        return nodeByTile[tile].Next;
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

    private void UpdateNodes() {
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
        System.Array.Sort(xArray);
        System.Array.Sort(yArray);

        nodes = new PathNode[xArray.Length, yArray.Length];
        spawnerNodes.Clear();
        destinationNodes.Clear();
        nodeByTile.Clear();
        foreach (ITile tile in allTiles) {
            int xIndex = System.Array.IndexOf(xArray, tile.transform.position.x);
            int yIndex = System.Array.IndexOf(yArray, tile.transform.position.y);
            if (nodes[xIndex, yIndex] != null) {
                Debug.LogError("Duplicated tile at " + tile.transform.position);
                continue;
            }
            nodes[xIndex, yIndex] = new PathNode {
                Tile = tile,
                Next = null,
                Destination = null,
                x = xIndex,
                y = yIndex,
            };
            nodeByTile[tile] = nodes[xIndex, yIndex];

            if (tile is SpawnerTile) {
                spawnerNodes.Add(nodes[xIndex, yIndex]);
            } else if (tile is DestinationTile) {
                destinationNodes.Add(nodes[xIndex, yIndex]);
                nodes[xIndex, yIndex].Destination = nodes[xIndex, yIndex];
            }
        }
    }

    private void UpdatePaths() {
        Queue<PathNode> queue = new();
        HashSet<PathNode> visitedSet = new();
        foreach (var node in destinationNodes) {
            queue.Enqueue(node);
            visitedSet.Add(node);
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
            foreach (int[] direction in directions) {
                int x = node.x + direction[0];
                int y = node.y + direction[1];
                if (x < 0 || x >= nodes.GetLength(0) || y < 0 || y >= nodes.GetLength(1)) {
                    continue;
                }
                // check corners for diagonal movement
                if (direction[0] != 0 && direction[1] != 0) {
                    if (nodes[x, node.y] == null || nodes[node.x, y] == null || nodes[x, node.y].Tile.IsPassable == false || nodes[node.x, y].Tile.IsPassable == false) {
                        continue;
                    }
                }

                PathNode nextNode = nodes[x, y];
                if (nextNode == null || nextNode.Tile.IsPassable == false || visitedSet.Contains(nextNode)) {
                    continue;
                }
                nextNode.Destination = node.Destination;
                nextNode.Next = node;
                queue.Enqueue(nextNode);
                visitedSet.Add(nextNode);
            }
        }
    }
}
