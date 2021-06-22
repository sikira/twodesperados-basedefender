using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelRandomCreater : MonoBehaviour
{
    public PlayerSettings pref;

    // Start is called before the first frame update
    void Awake()
    {
        pref = this.gameObject.GetComponent<PlayerSettings>();

        var newData = new LevelData();
        try
        {
            newData.SizeX = int.Parse(pref.gridX);
            newData.SizeY = int.Parse(pref.gridY);
            newData.ObstacleNumber = int.Parse(pref.gridY);
            newData.CurrentLevel = int.Parse(pref.currentLevel);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

        DebuggerGlobalSettings.UseDebugger = pref.DebuggerOn;
        newData.NumberOfEnemySpawner = 3 + Mathf.FloorToInt(LevelData.Instance.CurrentLevel / 3);

        var initalizer = new LevelInitalizer();
        initalizer.Init(newData);
    }


}
