using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelRefHolder : MonoBehaviour
{
    public LevelData levelData;
    public MainPlayer player;
    public PlayerBase playerBase;
    public Camera mainCamera;
    public EnemySpawner enemySpawner;

    public Tilemap FloorTileMap;
    public Tilemap ObstacleTileMap;
    public Tilemap EnemyTileMap;
    public TileBase FloorTileSample1;
    public TileBase ObstacleSample1;
    public TileBase SpawnerTileMap;



    // public Dictionary<> CompleteLevelNodes;
}
