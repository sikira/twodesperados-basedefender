using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DebuggerColorPalete
{
    public Color NeighboursColor = Color.cyan;
    public Color PositionColor = Color.blue;
    public Color ValidPathColor = Color.magenta;
    public Color OpenListColor = Color.green;
    public Color ClosedListColor = Color.red;
}
public class DebuggerPathfinding : MonoBehaviour
{
    public Transform NodePrefab;
    private Dictionary<Vector2Int, Transform> dic = new Dictionary<Vector2Int, Transform>();
    private GameObject nodeHolder;
    public void InitalizeMesh(Vector2Int[] mesh, Vector3[] worldPosition)
    {
        InitalizeHolders();

        for (var i = 0; i < mesh.Length; i++)
        {
            var node = Instantiate(NodePrefab, worldPosition[i] + new Vector3(0, 0, -0.2f), nodeHolder.transform.rotation, nodeHolder.transform);
            node.gameObject.SetActive(false);
            dic.Add(mesh[i], node);
        }
    }

    private void InitalizeHolders()
    {
        dic = new Dictionary<Vector2Int, Transform>();
        var nodeHolderName = "NodeHolder";
        var existingGameObject = GameObject.Find(nodeHolderName);
        if (existingGameObject != null)
            GameObject.DestroyImmediate(existingGameObject.gameObject);

        // Create Main Holders
        nodeHolder = new GameObject(nodeHolderName);
        nodeHolder.transform.parent = transform;
    }

    private DebuggerColorPalete GetPalet(int debugLayerNumber)
    {
        return new DebuggerColorPalete();
    }
    public void DebugSearch(int debugLayerNumber, BaseNode currentNode, List<BaseNode> openList, List<BaseNode> closedList, List<BaseNode> neighbourList)
    {
        foreach (var openNode in openList)
            MarkCurrentNode(openNode, GetPalet(debugLayerNumber).OpenListColor);

        foreach (var node in closedList)
            MarkCurrentNode(node, GetPalet(debugLayerNumber).ClosedListColor);

        foreach (var node in neighbourList)
            MarkCurrentNode(node, GetPalet(debugLayerNumber).NeighboursColor);

        MarkCurrentNode(currentNode, GetPalet(debugLayerNumber).PositionColor);
    }

    public void DebugPath(int debugLayerNumber, List<BaseNode> path)
    {
        foreach (var pathNode in path)
            MarkCurrentNode(pathNode, GetPalet(debugLayerNumber).ValidPathColor);
    }

    public void MarkCurrentNode(BaseNode node, Color color)
    {
        if (node != null)
            dic[node.Position]?.GetComponent<DebugerNode>()?.SetNode(color);
    }

    // internal void MarkNeigbours(List<BaseNode> neighbourList, Color color)
    // {
    //     foreach (var neighbour in neighbourList)
    //     {
    //         MarkCurrentNode(neighbour, color);
    //     }
    // }

    // internal void MarkPathNode(List<BaseNode> paths, Color color)
    // {
    //     foreach (var pathNode in paths)
    //     {
    //         MarkCurrentNode(pathNode, color); ;
    //     }
    // }
}
