using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelInitalizer : MonoBehaviour
{
    public Camera mainCamera;
    public LevelData levelData;
    public Tilemap FloorTileMap;
    public Tilemap ObstacleTileMap;
    public Tilemap PlayerTileMap;
    public TileBase FloorTileSample1;
    public TileBase ObstacleSample1;
    public TileBase PlayerSample1;
    public TileBase BaseTileMap;
    public TileBase SpawnerTileMap;

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
        

        
    }

    private void InitalizePlayerAndBase()
    {
        var baseArea = levelData.BaseArea;
        Vector3Int getRandomVector3() => new Vector3Int(UnityEngine.Random.Range(baseArea.x, baseArea.y),
                        UnityEngine.Random.Range(baseArea.width, baseArea.height), 0);

        var playerStartPosition = getRandomVector3();
        // set player position
        PlayerTileMap.SetTile(playerStartPosition, PlayerSample1);
        // set camera position
        mainCamera.transform.position = playerStartPosition + new Vector3Int(0, 0, -30);

        // spawn base
        Vector3Int basePosition = playerStartPosition;
        while (basePosition == playerStartPosition)
            basePosition = getRandomVector3();
        PlayerTileMap.SetTile(basePosition, BaseTileMap);

    }

    private void InitlaizeFloorAndBoundries()
    {
        for (int i = 0; i < levelData.SizeX; i++)
            for (int j = 0; j < levelData.SizeY; j++)
                if (i == 0 || j == 0 || i == levelData.SizeX - 1 || j == levelData.SizeY - 1)
                    FloorTileMap.SetTile(new Vector3Int(i, j, 0), ObstacleSample1);
                else
                    FloorTileMap.SetTile(new Vector3Int(i, j, 0), FloorTileSample1);
    }
}
