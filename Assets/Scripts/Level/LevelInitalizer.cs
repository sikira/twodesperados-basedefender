using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelInitalizer : MonoBehaviour
{
    public LevelData levelData;
    public Tilemap floor;

    void OnAwake()
    {
        InitalizeTiles();

    }

    private void InitalizeTiles()
    {
        InitlaizeFloorAndBoundries();

    }

    private void InitlaizeFloorAndBoundries()
    {
        
    }
}
