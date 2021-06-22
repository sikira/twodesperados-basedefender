using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INodePathfinderAlgo
{
    bool canWalkDiagonaly { get; set; }
    void CalculateAll();
    List<BaseNode> FindStep();
    void SetUp(Vector2Int startPosition, Vector2Int endPosition, RectInt map, List<Vector2Int> nonWalkablePositions);
    void SetUpDebugger(IDebuggerPathfinding debuggerPathfinding, int debugerLayer = -1);
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
