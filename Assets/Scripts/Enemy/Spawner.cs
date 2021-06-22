using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Vector3Int TilePositon = new Vector3Int();
    public List<Vector2Int> pathToBase = new List<Vector2Int>();
    public INodePathfinderAlgo pathfinderAlgo;


    void Start()
    {
        pathfinderAlgo = PathfindingAlgo.GetAlgo();

        var debuger = GameObject.FindObjectOfType<DebuggerPathfinding>();
        pathfinderAlgo.SetUpDebugger(debuger , debuger.GetId());

        var pshycs = GameObject.FindObjectOfType<PhysicsMonitor>();

        


    }




}
