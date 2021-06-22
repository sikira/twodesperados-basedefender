using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum SpawnState
{
    WAITTING, SPAWWING, COUNTING
}
public class EnemyPool
{
    public List<Enemy> allEnemiesInstances { get; set; } = new List<Enemy>();

}
public class EnemySpawnerControler : MonoBehaviour
{
    [SerializeField] public List<Spawner> spawnersList = new List<Spawner>();
    public Tilemap EnemyTilemap;

    private float timeBetweenWaves = 2f;
    private float waveCountdown = 0;
    public Transform spawnPrefab;
    public Transform enemyPrefab1;
    public Transform enemyPrefab2;
    private SpawnState spawnState = SpawnState.COUNTING;
    private List<Transform> enemiesPool = new List<Transform>();

    System.Random r = new System.Random();
    bool allEnemiesKilled = false;
    private float checkTime = 2f;

    void Start()
    {
        waveCountdown = timeBetweenWaves;
    }

    void Update()
    {
        if (allEnemiesKilled)
            return;

        if (spawnState == SpawnState.WAITTING)
        {
            // KO. Finish them all 
            checkTime -= Time.deltaTime;
            if (checkTime <= 0)
            {
                checkTime = 2f;
                var allEnemiesKilled = enemiesPool.Where(e => e.GetComponent<Enemy>()?.alive == true).Count() == 0;

                if (allEnemiesKilled)
                {
                    GameObject.FindObjectOfType<UiControl>()?.PlayerWon();

                }
            }
            return;
        }

        if (waveCountdown <= 0f)
        {
            if (spawnState != SpawnState.SPAWWING)
            {
                // Debug.Log("EnemySpawner:Pocni izbacivati");

                var wave = new SpawnWave();
                wave.count += LevelData.Instance.CurrentLevel;

                StartCoroutine(SpawnWave(wave));
            }
        }
        else
        {
            waveCountdown -= Time.deltaTime;
        }
    }

    bool AreAllEnemiedDead()
    {
        return false;
    }

    public void SetSpawnerPositions(List<Vector3Int> positions)
    {
        spawnersList = new List<Spawner>();

        foreach (var positons in positions)
        {
            // Debug.Log($"Spawner positions:{positons}");

            var startPosition = EnemyTilemap.GetCellCenterWorld(positons);
            var spawn = Instantiate(spawnPrefab, parent: this.gameObject.transform, rotation: this.gameObject.transform.rotation, position: startPosition);

            var spawnCom = spawn.GetComponent<Spawner>();
            spawnCom.TilePositon = positons;
            spawnersList.Add(spawnCom);
        }

    }

    void SpawnEnemy()
    {

        // Debug.Log($"EnemySpawner:SpawnEnemy {spawnersList.Count} - {System.DateTime.Now}");
        foreach (var pos in spawnersList)
        {
            var startPosition = EnemyTilemap.GetCellCenterWorld(pos.TilePositon);

            var enemy = GetEnemyInstance(this.gameObject.transform, this.gameObject.transform.rotation, startPosition);

            var sprite = enemy.GetComponent<SpriteRenderer>();
            sprite.sortingOrder = 22;

            //TODO: Add Enemy Precalculated Path From Spawn Element            
            // Enemy enemyInfo = enemy.GetComponent<Enemy>();   

            var atSet = new AttackSettings();
            atSet.hittingPower += (LevelData.Instance.CurrentLevel * 1);
            var health = 50 + (LevelData.Instance.CurrentLevel * 2);
            enemy.GetComponent<Enemy>()?.ReStartMe(atSet, health);

            var controls = enemy.GetComponent<KretanjePoPutanji>();
            controls.ClearMe();
            controls.walking = true;
            controls.speed = controls.WALK_SPEED;
            controls.CurrentTilePosition = (Vector2Int)pos.TilePositon;
            controls.tmap = EnemyTilemap;
        }
    }

    private Transform GetEnemyInstance(Transform mparent, Quaternion mrotation, Vector3 mposition)
    {
        var poolEnemy = enemiesPool.Where(e => e.GetComponent<Enemy>()?.alive == false).FirstOrDefault();
        if (poolEnemy != null)
        {
            Debug.Log("izvlaci iz pool-a");
            poolEnemy.transform.position = mposition;
            return poolEnemy;
        }

        var enemy = Instantiate(r.Next(0, 10) < 7 ? enemyPrefab1 : enemyPrefab2, parent: mparent, rotation: mrotation, position: mposition);
        enemiesPool.Add(enemy);
        return enemy;
    }

    IEnumerator SpawnWave(SpawnWave wave)
    {
        spawnState = SpawnState.SPAWWING;


        for (int i = 0; i < wave.count; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(wave.delay);
        }


        spawnState = SpawnState.WAITTING;

        yield break;
    }

}

public class SpawnWave
{
    public int count = 3;
    public float delay = 12f;

}
