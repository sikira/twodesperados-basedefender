using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        // pathfinderAlgo.SetUpDebugger(debuger, debuger.GetId());

        PhysicsMonitor pshycs2 = GameObject.FindObjectOfType<PhysicsMonitor>();
        pathfinderAlgo.SetUp((Vector2Int)TilePositon, pshycs2.endPosition, pshycs2.map, pshycs2.nonWalkablePositions);

        //TODO: slow down process and show it in debuger
        pathToBase = pathfinderAlgo.GetPath().ToList();

    }




}
