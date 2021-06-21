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

    private static int LastSizeX = 0;
    private static int LastSizeY = 0;


    private List<Vector3Int> spawnListPositions = new List<Vector3Int>();
    private List<Vector3Int> posibleSpawnListPositions = new List<Vector3Int>();
    public List<BaseObstacle> obstacleListPosition = new List<BaseObstacle>();

    public List<Vector3Int> freeSpaceOutsidePlayerBase = new List<Vector3Int>();

    void Awake()
    {
        // levelData = new LevelData();
        // InitalizeTiles();
    }
    public void Init(LevelData data)
    {
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

        InitalizeTiles();
    }



    private void InitalizeTiles()
    {
        ClearAll();
        InitlaizeFloorAndBoundries();
        InitalizePlayerAndBase();
        InitalizeEnemySpawnerPosition();
        InitalizeObstaclesAndDestObstables();
    }

    private void ClearAll()
    {
        //TODO:hardcoded
        LastSizeX = 100;
        LastSizeY = 100;
        for (int i = 0; i < LastSizeX; i++)
        {
            for (int j = 0; j < LastSizeY; j++)
            {
                levelRef.FloorTileMap.SetTile(new Vector3Int(i, j, 0), null);
                levelRef.EnemyTileMap.SetTile(new Vector3Int(i, j, 0), null);
                levelRef.ObstacleTileMap.SetTile(new Vector3Int(i, j, 0), null);
            }
        }
        posibleSpawnListPositions = new List<Vector3Int>();

    }

    private void InitalizeObstaclesAndDestObstables()
    {
        freeSpaceOutsidePlayerBase.RemoveAll(p => p.x >= levelData.BaseArea.x && p.x <= levelData.BaseArea.y
            && p.y >= levelData.BaseArea.width && p.y <= levelData.BaseArea.height);

        freeSpaceOutsidePlayerBase.RemoveAll(p => spawnListPositions.Any(s => s == p));

        System.Random r = new System.Random();

        int numOfObst = Mathf.FloorToInt(freeSpaceOutsidePlayerBase.Count * levelData.ObstaclePercent);

        for (int i = 0; i < numOfObst; i++)
        {
            var nextPos = r.Next(0, freeSpaceOutsidePlayerBase.Count);
            var pos = freeSpaceOutsidePlayerBase[nextPos];
            freeSpaceOutsidePlayerBase.RemoveAt(nextPos);

            var newPhicyMap = obstacleListPosition.Select(o => (Vector2Int)o.Position).ToList();
            newPhicyMap.Add((Vector2Int)pos);
            Debug.Log("Preracunaj");
            if (CanAllEnemiesWalkToBase(newPhicyMap))
            {
                Debug.Log("Ubaci");
                // add valid opstacle
                levelRef.ObstacleTileMap.SetTile(pos, levelRef.ObstacleSample1);
                obstacleListPosition.Add(new BaseObstacle((Vector2Int)pos));
            }
            else
            {
                Debug.Log("Izbaci");
            }

        }


    }

    private bool CanAllEnemiesWalkToBase(List<Vector2Int> nonWalkable)
    {
        foreach (var enemySpawnPosition in spawnListPositions)
        {
            INodePathfinderAlgo algo = PathfindingAlgo.GetAlgo();
            algo.SetUp((Vector2Int)enemySpawnPosition, (Vector2Int)levelData.basePosition, levelData, nonWalkable, 0, null);
            var path = algo.GetPath();
            if (path == null)
                return false;
        }
        return true;
    }

    private void InitalizeEnemySpawnerPosition()
    {
        var random = new System.Random();
        spawnListPositions = new List<Vector3Int>();



        //TODO: make better random funciton
        // **** Solution 1  ****//
        for (int i = 0; i < levelData.NumberOfEnemySpawner && posibleSpawnListPositions.Count() > 0; i++)
        {
            // better random resaults, there are other ways to get better res then this. :D
            for (int j = 0; j < 50; j++)
                random.Next(0, 100);

            var pos = random.Next(0, posibleSpawnListPositions.Count());
            if (pos < posibleSpawnListPositions.Count())
            {
                spawnListPositions.Add(posibleSpawnListPositions[pos]);
                posibleSpawnListPositions.RemoveAt(pos);
                try
                {
                    posibleSpawnListPositions.RemoveAt(pos-1);
                    posibleSpawnListPositions.RemoveAt(pos);
                }
                catch { }
            }
        }
        foreach (var pos in spawnListPositions)
            levelRef.EnemyTileMap.SetTile(pos, levelRef.SpawnerTileMap);

        levelRef.enemySpawner.SetSpawnerPositions(spawnListPositions.ToList());

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
        levelData.basePosition = playerStartPosition;
        while (levelData.basePosition == playerStartPosition)
            levelData.basePosition = getRandomVector3();

        // setting base position
        levelRef.playerBase.transform.position = levelRef.FloorTileMap.GetCellCenterWorld(levelData.basePosition); ;

        var middleVector = Vector3.Lerp(levelRef.player.transform.position, levelRef.playerBase.transform.position, .5f);
        levelRef.mainCamera.transform.position = middleVector + new Vector3(0, 0, -10);

    }

    private void InitlaizeFloorAndBoundries()
    {
        freeSpaceOutsidePlayerBase = new List<Vector3Int>();
        var lista = new List<Vector2Int>();
        var worldPosition = new List<Vector3>();
        LastSizeX = levelData.SizeX;
        LastSizeY = levelData.SizeY;

        for (int i = 0; i < levelData.SizeX; i++)
            for (int j = 0; j < levelData.SizeY; j++)
            {
                lista.Add(new Vector2Int(i, j));
                worldPosition.Add(levelRef.FloorTileMap.GetCellCenterWorld(new Vector3Int(i, j, 0)));

                if (i == 0 || j == 0 || i == levelData.SizeX - 1 || j == levelData.SizeY - 1)
                {

                    // creating outside wall
                    levelRef.ObstacleTileMap.SetTile(new Vector3Int(i, j, 0), levelRef.ObstacleSample1);
                    obstacleListPosition.Add(new BaseObstacle(new Vector2Int(i, j)));
                }
                else
                {
                    // getting posible spawn positions
                    if (i == 1 || j == 1 || i == levelData.SizeX - 2 || j == levelData.SizeY - 2)
                        posibleSpawnListPositions.Add(new Vector3Int(i, j, 0));
                    // setting basic floor
                    levelRef.FloorTileMap.SetTile(new Vector3Int(i, j, 0), levelRef.FloorTileSample1);
                    freeSpaceOutsidePlayerBase.Add(new Vector3Int(i, j, 0));
                }
            }

        GameObject.FindObjectOfType<DebuggerPathfinding>()?.InitalizeMesh(lista.ToArray(), worldPosition.ToArray());
    }
}
