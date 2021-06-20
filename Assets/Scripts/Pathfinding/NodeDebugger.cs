using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeDebugger : MonoBehaviour
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

    internal void ShowCurrentNode(BaseNode lowestFCostNode)
    {
        dic[lowestFCostNode.Position]?.gameObject.SetActive(true);
    }

    internal void ShowNeigbours(List<BaseNode> neighbourList)
    {
        foreach (var neighbour in neighbourList)
        {
            dic[neighbour.Position]?.gameObject.SetActive(true);
        }
    }
}
