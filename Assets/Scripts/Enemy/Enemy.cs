using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySettings
{
    public int hittingPower = 5;
    public float HitRate = 1.25f;
    public float currentHitRate = 1f;

}

public class Enemy : MonoBehaviour, IFakeTriggerComponent
{
    public EnemySettings settings = new EnemySettings();
    public static int enemyIdCounter = 0;
    private string enemyId = "";
    public static int timeStopCounter = 0;
    private bool debuging = false;
    private KretanjePoPutanji controls;
    Vector2Int endPosition = new Vector2Int();
    Vector2Int currentTarget = new Vector2Int(-1, -1);
    Vector2Int nextCurrentTarget = new Vector2Int(-2, -2);
    List<Vector2Int> currentPath = new List<Vector2Int>();
    public INodePathfinderAlgo pathfinderAlgo;
    PhysicsMonitor physicsMonitor;
    int maxStep;
    public int ViewSoundTriggerId = 1;
    public int AttackTriggerId = 2;
    private GameObject chasingTarget = null;





    void Start()
    {
        enemyIdCounter++;
        enemyId = enemyIdCounter.ToString();

        pathfinderAlgo = PathfindingAlgo.GetAlgo();

        physicsMonitor = GameObject.FindObjectOfType<PhysicsMonitor>();
        endPosition = physicsMonitor.endPosition;
        nextCurrentTarget = endPosition;

        controls = this.gameObject.GetComponent<KretanjePoPutanji>();

        var debuger = GameObject.FindObjectOfType<DebuggerPathfinding>();

        if (DebuggerGlobalSettings.UseDebugger)
        {
            pathfinderAlgo.SetUpDebugger(debuger, debuger.GetId());
        }

    }

    private void OnPshycsMapChangeUpdate()
    {
        var castAsVector = physicsMonitor.nonWalkablePositions?.Select(v => new Vector2Int(v.x, v.y))?.ToList();
        maxStep = castAsVector?.Count ?? 300;
        pathfinderAlgo.SetUp(controls.CurrentTilePosition, currentTarget, physicsMonitor.map, castAsVector ?? new List<Vector2Int>());
    }


    public void ObjectEntered(GameObject obj, int triggerId)
    {
        if (ViewSoundTriggerId == triggerId)
        {
            ChasePlayer(obj);
        }
        else if (AttackTriggerId == triggerId)
        {
            IHittableObject hitTarget = obj.GetComponent<IHittableObject>();
            hitTarget?.HitMe(settings.hittingPower);
        }
    }
    public void ObjectStay(GameObject obj, int triggerId)
    {
        if (AttackTriggerId == triggerId)
        {
            IHittableObject hitTarget = obj.GetComponent<IHittableObject>();
            if (settings.currentHitRate > settings.HitRate)
            {
                settings.currentHitRate = 0;
                hitTarget?.HitMe(settings.hittingPower);
            }
        }
    }


    public void ObjectExited(GameObject obj, int triggerId)
    {
        if (ViewSoundTriggerId == triggerId)
        {
            GoToBase();
        }
        else if (AttackTriggerId == triggerId)
        {

        }
    }

    private void GoToBase()
    {
        chasingTarget = null;
        controls.speed = controls.WALK_SPEED;
        nextCurrentTarget = endPosition;
    }
    private void ChasePlayer(GameObject gameObject)
    {
        controls.speed = controls.RUN_SPEED;
        chasingTarget = gameObject;
    }
    private Vector2Int GetClosesTile(GameObject chasingTarget)
    {
        //TODO: slow slow slow
        var tMap = GameObject.FindObjectOfType<LevelRefHolder>()?.EnemyTileMap;
        return (Vector2Int)tMap.WorldToCell(chasingTarget.transform.position);

    }
    // Update is called once per frame
    void Update()
    {
        settings.currentHitRate += Time.deltaTime;

        if (chasingTarget != null)
        {
            nextCurrentTarget = GetClosesTile(chasingTarget);
        }

        if (currentTarget != nextCurrentTarget)
        {
            // find new path
            currentTarget = nextCurrentTarget;
            OnPshycsMapChangeUpdate();
            if (DebuggerGlobalSettings.UseDebugger)
            {
                StartCoroutine(ShowPathDebuging());
            }
            else
            {
                currentPath = pathfinderAlgo.GetPath().ToList();
                controls.UpdatePath(currentPath);
            }
        }



        IEnumerator ShowPathDebuging()
        {
            debuging = true;
            timeStopCounter++;
            Time.timeScale = 0;

            float corutineSpeed = .04f;

            for (int i = 0; i < maxStep; i++)
            {
                var nextCalulatePath = pathfinderAlgo.FindStep();
                if (nextCalulatePath != null)
                {
                    currentPath = nextCalulatePath.Select(n => n.Position).ToList();
                    corutineSpeed = 1.5f;
                    i = maxStep;
                }
                yield return new WaitForSecondsRealtime(corutineSpeed);
            }
            controls.UpdatePath(currentPath);
            pathfinderAlgo.CleanDebugger();
            debuging = false;
            timeStopCounter--;
            if (timeStopCounter == 0)
                Time.timeScale = 1;

            yield break;
        }
    }


}
