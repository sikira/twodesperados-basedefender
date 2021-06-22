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

    void Start()
    {
        waveCountdown = timeBetweenWaves;
    }

    void Update()
    {
        if (spawnState == SpawnState.WAITTING)
        {
            // KO. Finish them all 


            return;
        }

        if (waveCountdown <= 0f)
        {
            if (spawnState != SpawnState.SPAWWING)
            {
                Debug.Log("EnemySpawner:Pocni izbacivati");
                StartCoroutine(SpawnWave(new SpawnWave()));

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
            var enemy = Instantiate(enemyPrefab1, parent: this.gameObject.transform, rotation: this.gameObject.transform.rotation, position: startPosition);

            var sprite = enemy.GetComponent<SpriteRenderer>();
            sprite.sortingOrder = 22;

            //TODO: add enemy starting targer and path from spawn            
            // Enemy enemyInfo = enemy.GetComponent<Enemy>();           

            var controls = enemy.GetComponent<KretanjePoPutanji>();
            controls.CurrentTilePosition = (Vector2Int)pos.TilePositon;
            controls.tmap = EnemyTilemap;
        }
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
    public int count = 1;
    public float delay = 5f;

}
