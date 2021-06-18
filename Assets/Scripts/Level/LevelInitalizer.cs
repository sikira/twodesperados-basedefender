using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelInitalizer : MonoBehaviour
{
    public LevelData levelData;
    public Tilemap FloorTileMap;
    public TileBase FloorTileSample1;
    public TileBase ObstacleSample1;

    void Awake()
    {
        levelData = new LevelData();
        InitalizeTiles();
    }

    private void InitalizeTiles()
    {
        InitlaizeFloorAndBoundries();

    }

    private void InitlaizeFloorAndBoundries()
    {
        for (int i = 0; i < levelData.SizeX; i++)
            for (int j = 0; j < levelData.SizeY; j++)
            {
                FloorTileMap.SetTile(new Vector3Int(i, j, 0), FloorTileSample1);
            }

    }
}
