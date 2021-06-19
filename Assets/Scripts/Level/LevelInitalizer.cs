using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelInitalizer : MonoBehaviour
{
    public MainPlayer player;
    public PlayerBase playerBase;
    public Camera mainCamera;
    public EnemySpawner enemySpawner;

    public LevelData levelData;
    public Tilemap FloorTileMap;
    public Tilemap ObstacleTileMap;
    public Tilemap EnemyTileMap;
    public TileBase FloorTileSample1;
    public TileBase ObstacleSample1;
    public TileBase SpawnerTileMap;


    private List<Vector3Int> spawnListPosition = new List<Vector3Int>();

    void Awake()
    {
        levelData = new LevelData();
        InitalizeTiles();
    }

    private void InitalizeTiles()
    {
        InitlaizeFloorAndBoundries();
        InitalizePlayerAndBase();
        InitalizeEnemySpawner();
        InitalizeObstaclesAndDestObstables();
    }

    private void InitalizeObstaclesAndDestObstables()
    {


    }

    private void InitalizeEnemySpawner()
    {
        var random = new System.Random();
        //TODO: make better random funciton
        // **** Solution 1  ****//
        for (int i = 0; i < levelData.NumberOfEnemySpawner && spawnListPosition.Count() > 0; i++)
        {
            // better random resaults, there other ways to get better res. :D
            for (int j = 0; j < 50; j++)
                random.Next(0, 100);

            var pos = random.Next(0, spawnListPosition.Count());
            if (pos < spawnListPosition.Count())
            {
                enemySpawner.spawnerPosition.Add(spawnListPosition[pos]);
                spawnListPosition.RemoveAt(pos);
            }
        }
        foreach (var pos in enemySpawner.spawnerPosition)
            EnemyTileMap.SetTile(pos, SpawnerTileMap);

        // **** Solution 2  ****//
        // while (spawnListPosition.Count() > levelData.NumberOfEnemySpawner)
        //     spawnListPosition.RemoveAt(random.Next(0, spawnListPosition.Count()));
        // foreach (var pos in spawnListPosition)
        //     EnemyTileMap.SetTile(pos, SpawnerTileMap);

        // **** Solution 3  ****//
        // var spawnerPosition = new List<Vector3Int>();
        // var stepPosition = 0;
        // for (int i = 0; i < levelData.NumberOfEnemySpawner && spawnListPosition.Count() > 0; i++)
        // {
        //     stepPosition += random.Next(5, spawnListPosition.Count() + 7);
        //     var pos = stepPosition % spawnListPosition.Count();
        //     spawnerPosition.Add(spawnListPosition[pos]);
        //     spawnListPosition.RemoveAt(pos);
        // }
        // foreach (var pos in spawnerPosition)
        //     EnemyTileMap.SetTile(pos, SpawnerTileMap);

    }

    private void InitalizePlayerAndBase()
    {
        Vector3Int getRandomVector3() => new Vector3Int(UnityEngine.Random.Range(levelData.BaseArea.x, levelData.BaseArea.y),
                UnityEngine.Random.Range(levelData.BaseArea.width, levelData.BaseArea.height), 0);

        var playerStartPosition = getRandomVector3();
        // setting player position
        // var playerWorldPositoin = FloorTileMap.GetCellCenterWorld(playerStartPosition);
        // player.GetComponent<Rigidbody2D>().MovePosition(new Vector2(playerWorldPositoin.x, playerWorldPositoin.y));
        player.GetComponent<Rigidbody2D>().MovePosition(FloorTileMap.GetCellCenterWorld(playerStartPosition));


        // spawn base
        Vector3Int basePosition = playerStartPosition;
        while (basePosition == playerStartPosition)
            basePosition = getRandomVector3();

        // setting base position
        // playerBase.GetComponent<Rigidbody2D>().MovePosition(new Vector2(basePosition.x, basePosition.y));
        playerBase.transform.position = FloorTileMap.GetCellCenterWorld(basePosition); ;

    }

    private void InitlaizeFloorAndBoundries()
    {
        for (int i = 0; i < levelData.SizeX; i++)
            for (int j = 0; j < levelData.SizeY; j++)
            {

                if (i == 0 || j == 0 || i == levelData.SizeX - 1 || j == levelData.SizeY - 1)
                    // creating outside wall
                    ObstacleTileMap.SetTile(new Vector3Int(i, j, 0), ObstacleSample1);
                else
                {
                    // getting posible spawn positions
                    if (i == 1 || j == 1 || i == levelData.SizeX - 2 || j == levelData.SizeY - 2)
                        spawnListPosition.Add(new Vector3Int(i, j, 0));
                    // setting basic floor
                    FloorTileMap.SetTile(new Vector3Int(i, j, 0), FloorTileSample1);
                }
            }
    }
}
