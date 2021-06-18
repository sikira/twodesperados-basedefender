using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class LevelData
{
    private const int minSizeX = 10;
    private const int minSizeY = 10;
    private const int maxSizeX = 666;
    private const int maxSizeY = 666;
    private int sizeX = 25;
    private int sizeY = 20;

    public int SizeX
    {
        get => sizeX;
        set
        {
            if (value >= minSizeX && value <= maxSizeX) sizeX = value;
        }
    }
    public int SizeY
    {
        get => sizeY;
        set
        {
            if (value >= minSizeY && value <= maxSizeY) sizeY = value;
        }
    }

    public RectInt BaseArea
    {
        get
        {
            var centerPoint = new Point(SizeX / 2, SizeY / 2);
            //TODO: make offset array 1/5 of map or 15 max;

            var minAreaSize = Mathf.Min(SizeX, SizeY);

            var offsetArea = minAreaSize / 5 > 15 ? 15 : Mathf.FloorToInt(minAreaSize / 5);
            if (offsetArea < 2)
                offsetArea = 2;

            return new RectInt(centerPoint.X - offsetArea, centerPoint.X + offsetArea, centerPoint.Y - offsetArea, centerPoint.Y + offsetArea);
            // return new Vector3Int[]{
            //     new Vector3Int(centerPoint.X - offsetArea, centerPoint.Y - offsetArea,0),
            //     new Vector3Int(centerPoint.X + offsetArea, centerPoint.Y - offsetArea,0),
            //     new Vector3Int(centerPoint.X + offsetArea, centerPoint.Y + offsetArea,0),
            //     new Vector3Int(centerPoint.X - offsetArea, centerPoint.Y + offsetArea,0)
            // };
        }
    }


}
