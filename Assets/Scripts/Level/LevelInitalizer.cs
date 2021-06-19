using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelInitalizer
{
    public LevelData levelData;
    private GameObject mainLevelHolder;
    private LevelRefHolder levelRef;


    private List<Vector3Int> spawnListPosition = new List<Vector3Int>();

    void Awake()
    {
        // levelData = new LevelData();
        // InitalizeTiles();
    }
    public void Init(LevelData data)
    {
        Debug.Log("Level Initalizer: Init()");
        levelData = data;
        levelRef = GameObject.FindObjectOfType<LevelRefHolder>();
        InitLevel();
    }
    private void InitLevel()
    {
        if (levelData is null || levelRef is null)
        {
            Debug.Log($"Podaci nisu initazlivoani { levelData } - { levelRef }");
            throw new ArgumentNullException("Moraju biti posavljeni");
        }

        // InitalizeObjects();
        Debug.Log("Level Initalizer: Init TIles()");
        InitalizeTiles();
    }

    // private void InitalizeObjects()
    // {
    //     var existingGameObject = GameObject.Find(LevelData.MAIN_GAMEOBJECT_NAME);
    //     if (existingGameObject != null)
    //         GameObject.DestroyImmediate(existingGameObject.gameObject);

    //     // Create Main Holders
    //     mainLevelHolder = new GameObject(LevelData.MAIN_GAMEOBJECT_NAME);
    //     mainLevelHolder.transform.position = Vector3.zero;
    // }

    private void InitalizeTiles()
    {
        ClearAll();
        InitlaizeFloorAndBoundries();
        InitalizePlayerAndBase();
        InitalizeEnemySpawner();
        InitalizeObstaclesAndDestObstables();
    }

    private void ClearAll()
    {
        for (int i = 0; i < levelData.SizeX; i++)
        {
            for (int j = 0; j < levelData.SizeY; j++)
            {
                levelRef.FloorTileMap.SetTile(new Vector3Int(i, j, 0), null);
                levelRef.EnemyTileMap.SetTile(new Vector3Int(i, j, 0), null);
                levelRef.ObstacleTileMap.SetTile(new Vector3Int(i, j, 0), null);
            }
        }
        spawnListPosition = new List<Vector3Int>();

    }

    private void InitalizeObstaclesAndDestObstables()
    {


    }

    private void InitalizeEnemySpawner()
    {
        var random = new System.Random();
        //TODO: make better random funciton
        // **** Solution 1  ****//
        var spawnerPositions = new List<Vector3Int>();
        for (int i = 0; i < levelData.NumberOfEnemySpawner && spawnListPosition.Count() > 0; i++)
        {
            // better random resaults, there other ways to get better res. :D
            for (int j = 0; j < 50; j++)
                random.Next(0, 100);

            var pos = random.Next(0, spawnListPosition.Count());
            if (pos < spawnListPosition.Count())
            {
                spawnerPositions.Add(spawnListPosition[pos]);
                spawnListPosition.RemoveAt(pos);
            }
        }
        foreach (var pos in spawnerPositions)
            levelRef.EnemyTileMap.SetTile(pos, levelRef.SpawnerTileMap);

        levelRef.enemySpawner.SetSpawnerPositions(spawnerPositions.ToList());

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
        levelRef.player.transform.position = levelRef.FloorTileMap.GetCellCenterWorld(playerStartPosition);


        // spawn base
        Vector3Int basePosition = playerStartPosition;
        while (basePosition == playerStartPosition)
            basePosition = getRandomVector3();

        // setting base position
        levelRef.playerBase.transform.position = levelRef.FloorTileMap.GetCellCenterWorld(basePosition); ;

        var middleVector = Vector3.Lerp(levelRef.player.transform.position, levelRef.playerBase.transform.position, .5f);
        levelRef.mainCamera.transform.position = middleVector + new Vector3(0, 0, -10);
    }

    private void InitlaizeFloorAndBoundries()
    {
        for (int i = 0; i < levelData.SizeX; i++)
            for (int j = 0; j < levelData.SizeY; j++)
            {

                if (i == 0 || j == 0 || i == levelData.SizeX - 1 || j == levelData.SizeY - 1)
                    // creating outside wall
                    levelRef.ObstacleTileMap.SetTile(new Vector3Int(i, j, 0), levelRef.ObstacleSample1);
                else
                {
                    // getting posible spawn positions
                    if (i == 1 || j == 1 || i == levelData.SizeX - 2 || j == levelData.SizeY - 2)
                        spawnListPosition.Add(new Vector3Int(i, j, 0));
                    // setting basic floor
                    levelRef.FloorTileMap.SetTile(new Vector3Int(i, j, 0), levelRef.FloorTileSample1);
                }
            }
    }
}
