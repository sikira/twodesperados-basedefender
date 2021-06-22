using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private KretanjePoPutanji controls;
    Vector2Int endPosition = new Vector2Int();
    Vector2Int currentTarget = new Vector2Int(-1, -1);
    Vector2Int nextCurrentTarget = new Vector2Int();
    List<Vector2Int> currentPath = new List<Vector2Int>();
    public INodePathfinderAlgo pathfinderAlgo;
    PhysicsMonitor physicsMonitor;

    void Start()
    {
        controls = this.gameObject.GetComponent<KretanjePoPutanji>();
        pathfinderAlgo = PathfindingAlgo.GetAlgo();

        var debuger = GameObject.FindObjectOfType<DebuggerPathfinding>();
        pathfinderAlgo.SetUpDebugger(debuger, debuger.GetId());

        physicsMonitor = GameObject.FindObjectOfType<PhysicsMonitor>();
        endPosition = physicsMonitor.endPosition;
        
    }


    // Update is called once per frame
    void Update()
    {
        // TODO: optimizuj provjeru da se ne desava stalno i uvijek :D
        if (currentTarget != nextCurrentTarget)
        {
            // find new path
            currentTarget = nextCurrentTarget;
            currentPath = pathfinderAlgo.GetPath().ToList();
            controls.UpdatePath(currentPath);
            
        }


    }
}
