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
                if (i == 0 || j == 0 || i == levelData.SizeX - 1 || j == levelData.SizeY - 1)
                {
                    FloorTileMap.SetTile(new Vector3Int(i, j, 0), ObstacleSample1);
                }
                else
                {
                    FloorTileMap.SetTile(new Vector3Int(i, j, 0), FloorTileSample1);
                }

            }



    }
}
