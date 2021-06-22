using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum SpawnState
{
    WAITTING, SPAWWING, COUNTING
}
public class EnemySpawner : MonoBehaviour
{
    private List<Vector3Int> spawnerPosition = new List<Vector3Int>();
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
        spawnerPosition = newPositons;
    }

    void SpawnEnemy()
    {
        Debug.Log($"EnemySpawner:SpawnEnemy");
        foreach (var pos in spawnerPosition)
        {
            Debug.Log($"EnemySpawner:izbaci mene {pos}");


            var startPosition = EnemyTilemap.GetCellCenterWorld(pos);
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
    public int count = 5;
    public float delay = 2f;

}
