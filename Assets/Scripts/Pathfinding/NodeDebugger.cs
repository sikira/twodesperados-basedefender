using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeDebugger : MonoBehaviour
{
    public Transform NodePrefab;
    private Dictionary<Vector3Int, Transform> dic = new Dictionary<Vector3Int, Transform>();
    private GameObject nodeHolder;
    public void InitalizeMesh(Vector3Int[] mesh, Vector3[] worldPosition)
    {
        InitalizeHolders();

        for (var i = 0; i < mesh.Length; i++)
        {
            var node = Instantiate(NodePrefab, worldPosition[i], nodeHolder.transform.rotation, nodeHolder.transform);
            dic.Add(mesh[i], node);
        }
    }

    private void InitalizeHolders()
    {
        dic = new Dictionary<Vector3Int, Transform>();
        var nodeHolderName = "NodeHolder";
        var existingGameObject = GameObject.Find(nodeHolderName);
        if (existingGameObject != null)
            GameObject.DestroyImmediate(existingGameObject.gameObject);

        // Create Main Holders
        nodeHolder = new GameObject(nodeHolderName);
        nodeHolder.transform.position = Vector3.zero;
        nodeHolder.transform.parent = transform;

    }

}
