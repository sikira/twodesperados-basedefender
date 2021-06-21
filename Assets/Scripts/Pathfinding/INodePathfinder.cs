using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INodePathfinderAlgo
{
    void CalculateAll();
    List<BaseNode> FindStep();
    void SetUp(Vector2Int startPosition, Vector2Int endPosition, LevelData data, List<Vector2Int> nonWalkablePositions, int debugLayer, IDebuggerPathfinding debuggerPathfinding);
    Vector2Int[] GetPath();
}

public class PathfindingAlgo
{

    //TODO: add random num
    public static float algoType = 0f;
    public static INodePathfinderAlgo GetAlgo()
    {
        if (algoType < .5f)
        {
            return new AStarAlgo();
        }
        else
        {
            return new AStarAlgo();
        }
    }

}
