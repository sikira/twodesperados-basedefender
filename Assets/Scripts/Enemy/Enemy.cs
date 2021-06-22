using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static int enemyIdCounter = 0;
    private string enemyId = "";
    public static int timeStopCounter = 0;
    private bool debuging = false;
    private KretanjePoPutanji controls;
    Vector2Int endPosition = new Vector2Int();
    Vector2Int currentTarget = new Vector2Int(-1, -1);
    Vector2Int nextCurrentTarget = new Vector2Int();
    List<Vector2Int> currentPath = new List<Vector2Int>();
    public INodePathfinderAlgo pathfinderAlgo;
    PhysicsMonitor physicsMonitor;

    void Start()
    {
        enemyIdCounter++;
        enemyId = enemyIdCounter.ToString();

        pathfinderAlgo = PathfindingAlgo.GetAlgo();

        controls = this.gameObject.GetComponent<KretanjePoPutanji>();

        var debuger = GameObject.FindObjectOfType<DebuggerPathfinding>();
        pathfinderAlgo.SetUpDebugger(debuger, debuger.GetId());


        OnPshycsMapChangeUpdate();
    }

    private void OnPshycsMapChangeUpdate()
    {
        physicsMonitor = GameObject.FindObjectOfType<PhysicsMonitor>();
        endPosition = physicsMonitor.endPosition;
        var castAsVector = physicsMonitor.nonWalkablePositions.Select(v => new Vector2Int(v.x, v.y)).ToList();
        pathfinderAlgo.SetUp(controls.CurrentTilePosition, physicsMonitor.endPosition, physicsMonitor.map, castAsVector);
    }




    // Update is called once per frame
    void Update()
    {

        if (currentTarget != nextCurrentTarget)
        {
            // find new path
            currentTarget = nextCurrentTarget;

            StartCoroutine(ShowPathDebuging());
            // currentPath = pathfinderAlgo.GetPath().ToList();

            controls.UpdatePath(currentPath);
        }



        IEnumerator ShowPathDebuging()
        {
            debuging = true;
            timeStopCounter++;
            Time.timeScale = 0;

            Debug.Log($"{enemyId}-Usao u showDebuging: " + DateTime.Now);

            //TODO: dodati ukupan broj kocikica;
            var maxStep = 500;
            for (int i = 0; i < maxStep; i++)
            {

                // Debug.Log($"{enemyId}- MAKE STEP " + DateTime.Now);
                var nextCalulatePath = pathfinderAlgo.FindStep();
                if (nextCalulatePath != null)
                {
                    Debug.Log($"{enemyId}- FIND PATH {nextCalulatePath.Count} - " + DateTime.Now);

                    currentPath = nextCalulatePath.Select(n => n.Position).ToList();
                    i = maxStep;
                }

                yield return new WaitForSecondsRealtime(.5f);
            }

            Debug.Log($"{enemyId}-Izasao u showDebuging: " + DateTime.Now);
            debuging = false;
            timeStopCounter--;
            if (timeStopCounter == 0)
                Time.timeScale = 1;

            yield break;
        }
    }
}
