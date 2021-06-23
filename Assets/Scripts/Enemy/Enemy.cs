using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AttackSettings
{
    public int hittingPower = 4;
    public float HitRate = 1.25f;
    public float currentHitRate = 1f;

}

public class Enemy : MonoBehaviour, IFakeTriggerComponent, IHittableObject
{
    public AttackSettings settings = new AttackSettings();
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
    private int startHealth = 100;
    private int _health = 100;
    public bool alive = true;

    public int Health { get => _health; private set => _health = value; }
    private Tilemap tMap;
    private TileBase bloodTile;
    private HealtBar healthBar;

    void Start()
    {
        SetMeUp();

    }

    private void SetMeUp()
    {
        tMap = GameObject.FindObjectOfType<LevelRefHolder>()?.EnemyTileMap;
        bloodTile = GameObject.FindObjectOfType<LevelRefHolder>()?.BloodTileSampe1;

        healthBar = this.gameObject.GetComponentInChildren<HealtBar>();
        healthBar?.SetPercent(1, 1);

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

    public void ReStartMe(AttackSettings settings, int enemyHealth)
    {
        SetMeUp();
        this.settings = settings;
        _health = enemyHealth;
        startHealth = enemyHealth;
        GetComponent<BoxCollider2D>().enabled = true;
        alive = true;
        this.gameObject.SetActive(true);
        chasingTarget = null;
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

    public void HitMe(int power)
    {
        if (debuging)
            return;

        Health -= power;

        healthBar?.SetPercent(Health, startHealth);

        if (Health <= 0)
        {
            Killed();
        }

        // Debug.Log($"Auuuuch {Health} -  {alive}");
    }

    private void Killed()
    {
        alive = false;
        controls.Stop();
        chasingTarget = null;

        GetComponent<BoxCollider2D>().enabled = false;
        this.gameObject.SetActive(false);

        tMap?.SetTile((Vector3Int)controls.CurrentTilePosition, bloodTile);
    }

    public void HealMe(int power)
    {
        Health += power;
        if (Health > 100)
            Health = 100;
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
        return (Vector2Int)tMap.WorldToCell(chasingTarget.transform.position);
    }
    // Update is called once per frame
    void Update()
    {
        if (!alive)
            return;

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
                currentPath = nextCalulatePath?.Select(n => n.Position)?.ToList();
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
