using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class DebuggerColorPaletes
{
    internal static List<Color> colors = new List<Color>(){
            Color.cyan , Color.blue , Color.green , Color.red , Color.yellow , Color.white , Color.black , Color.grey , Color.magenta , Color.yellow
    };

    public static List<DebuggerColorPalete> _palet = null;
    public static List<DebuggerColorPalete> CreatePalets()
    {
        return colors.Select(c => new DebuggerColorPalete()
        {
            NeighboursColor = new Color(c.r, c.g, c.b, .25f),
            PositionColor = new Color(c.r, c.g, c.b, 1f),
            ValidPathColor = new Color(c.r, c.g, c.b, 1f),
            OpenListColor = new Color(c.r, c.g, c.b, .75f),
            ClosedListColor = new Color(c.r, c.g, c.b, .5f)
        }).ToList();
    }
    public static List<DebuggerColorPalete> palet
    {
        get
        {
            if (_palet == null)
                _palet = CreatePalets();
            return _palet;
        }
    }

}
public class DebuggerColorPalete
{
    public Color NeighboursColor = Color.cyan;
    public Color PositionColor = Color.blue;
    public Color ValidPathColor = Color.magenta;
    public Color OpenListColor = Color.green;
    public Color ClosedListColor = Color.red;

}

public interface IDebuggerPathfinding
{
    void DebugPath(int layerNumber, List<BaseNode> path);
    void DebugSearch(int layerNumber, BaseNode currentNode, List<BaseNode> openList, List<BaseNode> closedList, List<BaseNode> neighbourList);
    void InitalizeMesh(Vector2Int[] mesh, Vector3[] worldPosition);
    void MarkCurrentNode(BaseNode node, int layerNumber, Color color);
    void Clear(int debugLayerNumber);
    int GetId();
}

public class DebuggerPathfinding : MonoBehaviour, IDebuggerPathfinding
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

    private DebuggerColorPalete GetPalet(int layerNumber)
    {
        return DebuggerColorPaletes.palet[layerNumber];
    }
    public void DebugSearch(int layerNumber, BaseNode currentNode, List<BaseNode> openList, List<BaseNode> closedList, List<BaseNode> neighbourList)
    {
        foreach (var node in neighbourList)
            MarkCurrentNode(node, layerNumber, GetPalet(layerNumber).NeighboursColor);

        foreach (var openNode in openList)
            MarkCurrentNode(openNode, layerNumber, GetPalet(layerNumber).OpenListColor);

        foreach (var node in closedList)
            MarkCurrentNode(node, layerNumber, GetPalet(layerNumber).ClosedListColor);

        MarkCurrentNode(currentNode, layerNumber, GetPalet(layerNumber).PositionColor);
    }

    public void DebugPath(int layerNumber, List<BaseNode> path)
    {
        Clear(layerNumber);

        foreach (var pathNode in path)
            MarkCurrentNode(pathNode, layerNumber, GetPalet(layerNumber).ValidPathColor);
    }

    public void MarkCurrentNode(BaseNode node, int layerNumber, Color color)
    {
        if (node != null)
            dic[node.Position]?.GetComponent<DebugerNode>()?.SetNode(layerNumber, color);
    }

    public void Clear(int layerNumber)
    {
        var allMarked = dic.Where(k => k.Value.GetComponent<DebugerNode>()?.IsLayerActive(layerNumber) == true).Select(k => k.Value.GetComponent<DebugerNode>());
        foreach (var mark in allMarked)
            mark.DeactivateMark(layerNumber);
    }

    private static int _id = -1;
    public int GetId()
    {
        return (_id++) / DebuggerColorPaletes.colors.Count;
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
