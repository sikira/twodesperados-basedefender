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

    public LevelData levelData;
    public Tilemap FloorTileMap;
    public Tilemap ObstacleTileMap;
    public Tilemap EnemyTileMap;
    public TileBase FloorTileSample1;
    public TileBase ObstacleSample1;
    public TileBase PlayerSample1;
    public TileBase BaseTileMap;
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
    }

    private void InitalizeEnemySpawner()
    {
        var random = new System.Random();
        // **** Solution 1  ****//
        var spawnerPosition = new List<Vector3Int>();
        for (int i = 0; i < levelData.NumberOfEnemySpawner && spawnListPosition.Count() > 0; i++)
        {
            // better random resoults, there other ways to create this. Use seed :D
            for (int j = 0; j < 50; j++)
                random.Next(0, 100);

            var pos = random.Next(0, spawnListPosition.Count());
            if (pos > spawnerPosition.Count())
            {
                spawnerPosition.Add(spawnListPosition[pos]);
                spawnListPosition.RemoveAt(pos);
            }
        }
        foreach (var pos in spawnerPosition)
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
        var baseArea = levelData.BaseArea;
        Vector3Int getRandomVector3() => new Vector3Int(UnityEngine.Random.Range(baseArea.x, baseArea.y),
                        UnityEngine.Random.Range(baseArea.width, baseArea.height), 0);

        var playerStartPosition = getRandomVector3();
        // set player position
        player.GetComponent<Rigidbody2D>().MovePosition(new Vector2(playerStartPosition.x, playerStartPosition.y));
        

        // spawn base
        Vector3Int basePosition = playerStartPosition;
        while (basePosition == playerStartPosition)
            basePosition = getRandomVector3();

        // playerBase.GetComponent<Rigidbody2D>().MovePosition(new Vector2(basePosition.x, basePosition.y));
        playerBase.transform.position = basePosition;

    }

    private void InitlaizeFloorAndBoundries()
    {
        for (int i = 0; i < levelData.SizeX; i++)
            for (int j = 0; j < levelData.SizeY; j++)
            {
                if (i == 0 || j == 0 || i == levelData.SizeX - 1 || j == levelData.SizeY - 1)
                    ObstacleTileMap.SetTile(new Vector3Int(i, j, 0), ObstacleSample1);
                else
                {
                    if (i == 1 || j == 1 || i == levelData.SizeX - 2 || j == levelData.SizeY - 2)
                        spawnListPosition.Add(new Vector3Int(i, j, 0));
                    FloorTileMap.SetTile(new Vector3Int(i, j, 0), FloorTileSample1);
                }
            }
    }
}
