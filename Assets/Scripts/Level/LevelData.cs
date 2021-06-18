using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour
{
    private const int minSizeX = 6;
    private const int minSizeY = 6;
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


}
