using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{
    private List<Vector3Int> spawnerPosition = new List<Vector3Int>();
    public Tilemap EnemyTilemap;


    public Transform enemyPrefab1;
    void Start()
    {
        SpawnEnemy();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetSpawnerPositions(List<Vector3Int> newPositons)
    {
        spawnerPosition = newPositons;
    }

    void SpawnEnemy()
    {
        foreach (var pos in spawnerPosition)
        {
            var startPosition = EnemyTilemap.GetCellCenterWorld(pos);
            var enemy = Instantiate(enemyPrefab1, parent: this.gameObject.transform, rotation: this.gameObject.transform.rotation, position: startPosition);

            var sprite = enemy.GetComponent<SpriteRenderer>();
            sprite.sortingOrder = 22;

        }
    }
}
