using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class LevelData
{
    public static readonly string MAIN_GAMEOBJECT_NAME = "MainGameEngine";

    private const int minSizeX = 10;
    private const int minSizeY = 10;
    private const int maxSizeX = 666;
    private const int maxSizeY = 666;
    [Range(10, 666)] private int sizeX = 25;
    [Range(10, 666)] private int sizeY = 20;

    private Nullable<RectInt> _baseArea = null;
    public int NumberOfEnemySpawner = 4;



    public int SizeX
    {
        get => sizeX;
        set
        {
            if (value >= minSizeX && value <= maxSizeX)
            {
                sizeX = value;
                _baseArea = null;
            }
        }
    }
    public int SizeY
    {
        get => sizeY;
        set
        {
            if (value >= minSizeY && value <= maxSizeY)
            {
                sizeY = value;
                _baseArea = null;
            }
        }
    }

    public RectInt BaseArea
    {
        get
        {
            if (_baseArea is null)
            {
                var centerPoint = new Point(SizeX / 2, SizeY / 2);
                //TODO: make offset array 1/5 of map or 15 max;

                var minAreaSize = Mathf.Min(SizeX, SizeY);

                var offsetArea = minAreaSize / 5 > 15 ? 15 : Mathf.FloorToInt(minAreaSize / 5);
                if (offsetArea < 2)
                    offsetArea = 2;

                _baseArea = new RectInt(centerPoint.X - offsetArea, centerPoint.X + offsetArea, centerPoint.Y - offsetArea, centerPoint.Y + offsetArea);
            }
            return _baseArea.GetValueOrDefault();
        }
    }


}
