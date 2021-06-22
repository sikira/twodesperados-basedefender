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

    public void SetSpawnerPositions(List<Vector3Int> newPositons)
    {
        foreach (var spawnerPod in spawnersList)
        {

        }

    }

    void SpawnEnemy()
    {
        Debug.Log($"EnemySpawner:SpawnEnemy {System.DateTime.Now}");
        foreach (var pos in spawnersList)
        {
            Debug.Log($"EnemySpawner:Unutar petlje {pos}");


            var startPosition = EnemyTilemap.GetCellCenterWorld(pos.TilePositon);
            var enemy = Instantiate(enemyPrefab1, parent: this.gameObject.transform, rotation: this.gameObject.transform.rotation, position: startPosition);



            var sprite = enemy.GetComponent<SpriteRenderer>();
            sprite.sortingOrder = 22;

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
    public int count = 3;
    public float delay = 4f;

}
