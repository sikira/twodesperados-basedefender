using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class AStarAlgo : INodePathfinder
{
    public int DebugLayerNumber = 0;
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;
    int maxWidth;
    int maxHeight;

    BaseNode startNode;
    BaseNode endNodeMark;
    private List<BaseNode> openList;
    private List<BaseNode> closedList;
    private List<BaseNode> nonWalkablesList = Enumerable.Range(0, 20).Select(n => new BaseNode(new Vector2Int(n, 4))).ToList();

    private List<BaseNode> completeMap;
    private bool canWalkDiagonaly = false;
    private readonly Vector2Int[] neighbourPositions = new Vector2Int[] { new Vector2Int(-1, 0), new Vector2Int(1, 0), new Vector2Int(0, -1), new Vector2Int(0, 1) };

    IDebuggerPathfinding debuger;

    public void Find(Vector2Int startPosition, Vector2Int endPosition, LevelData dataFake, int layer)
    {
        DebugLayerNumber = layer;
        nonWalkablesList.RemoveAt(9);
        nonWalkablesList.Add(new BaseNode(new Vector2Int(9, 4)));


        debuger = GameObject.FindObjectOfType<DebuggerPathfinding>();
        maxWidth = dataFake.SizeX;
        maxHeight = dataFake.SizeY;

        openList = new List<BaseNode>();
        closedList = new List<BaseNode>();
        completeMap = CompleteMesh();

        endNodeMark = GetNode(endPosition);

        startNode = GetNode(startPosition);
        startNode.G = 0;
        startNode.H = CalculateDistanceCost(startNode, endNodeMark);

        openList.Add(startNode);
    }

    public void CalculateAll()
    {
        while (openList.Count > 0)
            if (FindStep())
                break;
    }

    public bool FindStep()
    {
        if (openList.Count == 0)
        {
            Debug.Log("no open list");
            return false;
        }
        // Debug.Log("FindStep");

        BaseNode lowestFCostNode = openList.OrderBy(n => n.F).First();

        // Debug.Log($"lowestFCostNode:{lowestFCostNode}");

        if (lowestFCostNode == endNodeMark)
        {
            Debug.Log("Kraj pronadjeno sve!");
            var path = CalculatePath(lowestFCostNode);
            debuger?.DebugPath(DebugLayerNumber, path);

            return true;
        }

        openList.Remove(lowestFCostNode);
        closedList.Add(lowestFCostNode);

        // convert to IEnumeralble and use in foreach
        var neighbourList = GetNeighbourList(lowestFCostNode);

        debuger?.DebugSearch(DebugLayerNumber, lowestFCostNode, openList, closedList, neighbourList);


        foreach (var neighbourNode in neighbourList)
        {
            if (closedList.Exists(n => n.Position.x == neighbourNode.Position.x && n.Position.y == neighbourNode.Position.y))
                continue;

            int checkGCost = lowestFCostNode.G + CalculateDistanceCost(lowestFCostNode, neighbourNode);
            if (checkGCost < neighbourNode.G)
            {
                if (!canWalkDiagonaly && AreDiagonalNode(lowestFCostNode, neighbourNode) && GetWalkableNodeToDiagonaleNode(lowestFCostNode, neighbourNode) == null)
                {
                    continue;
                }

                // Debug.Log(lowestFCostNode);
                neighbourNode.Parent = lowestFCostNode;
                neighbourNode.G = checkGCost;
                neighbourNode.H = CalculateDistanceCost(neighbourNode, endNodeMark);

                //TODO: recheck if needed check for contains
                if (!openList.Contains(neighbourNode))
                    openList.Add(neighbourNode);
            }
        }
        return false;
    }

    private BaseNode GetWalkableNodeToDiagonaleNode(BaseNode currentNode, BaseNode neighbourNode)
    {
        return new List<BaseNode>(){
            GetWalkableNode(new Vector2Int(currentNode.Position.x, neighbourNode.Position.y)),
            GetWalkableNode(new Vector2Int(neighbourNode.Position.x, currentNode.Position.y))
        }.Where(a => a != null).FirstOrDefault();
    }
    private bool AreDiagonalNode(BaseNode currentNode, BaseNode neighbourNode)
    {
        return currentNode.Position.x - neighbourNode.Position.x != 0 && currentNode.Position.y - neighbourNode.Position.y != 0;
        // return Mathf.Abs(currentNode.Position.x - neighbourNode.Position.x) == 1
        // && Mathf.Abs(currentNode.Position.y - neighbourNode.Position.y) == 1;
    }

    private void p()
    {
        Debug.Log($"Find dimensions { maxWidth} {maxHeight}");
        Debug.Log($"completeMap { completeMap.Count}");
        Debug.Log($"openList { openList.Count}");
        Debug.Log($"closedList { closedList.Count}");
    }

    private List<BaseNode> CalculatePath(BaseNode endNodeLocal)
    {
        var testBreak = maxHeight * maxWidth;
        List<BaseNode> path = new List<BaseNode>();
        var currentNode = endNodeLocal;
        path.Add(currentNode);

        Debug.Log($"EndParent {currentNode.Parent} > {endNodeLocal.Parent}");

        while (currentNode.Parent != null && testBreak-- > 0)
        {
            Debug.Log(currentNode);
            Debug.Log(currentNode.Parent);
            if (!canWalkDiagonaly && AreDiagonalNode(currentNode, currentNode.Parent))
            {
                var node = GetWalkableNodeToDiagonaleNode(currentNode, currentNode.Parent);
                if (node == null)
                {
                    Debug.Log("BELAJ");
                }
                else
                {
                    path.Add(node);

                }
            }

            path.Add(currentNode.Parent);
            currentNode = currentNode.Parent;
        }
        Debug.Log($"Ukupno parenta { completeMap.Where(c => c.Parent != null).Count() }");
        var m = completeMap.Where(c => c.Parent != null).ToList();
        foreach (var mm in m)
        {
            Debug.Log($"> {mm} - {mm.Parent}");
        }

        path.Reverse();
        return path;
    }


    // private List<BaseNode> GetNeighbourListWithoutDiagonals(BaseNode currentNode)
    // {
    //     var neighbourList = new List<BaseNode>();
    //     foreach (var neighbour in neighbourPositions)
    //     {
    //         BaseNode goodNode = GetWalkableNode(currentNode.Position + neighbour);
    //         if (goodNode != null)
    //             neighbourList.Add(goodNode);
    //     }
    //     return neighbourList;
    // }
    private List<BaseNode> GetNeighbourList(BaseNode currentNode)
    {
        var neighbourList = new List<BaseNode>();
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0) continue;
                BaseNode goodNode = GetWalkableNode(currentNode.Position + new Vector2Int(i, j));
                if (goodNode != null)
                    neighbourList.Add(goodNode);
            }
        }
        return neighbourList;
    }

    private BaseNode GetNode(Vector2Int vecId) => completeMap.Where(n => n.Position.x == vecId.x && n.Position.y == vecId.y).FirstOrDefault();

    private BaseNode GetWalkableNode(Vector2Int vecId)
    {
        if (nonWalkablesList.Exists(n => n.Position.x == vecId.x && n.Position.y == vecId.y))
            return null;

        // return only valid param
        return completeMap.Where(n => n.Position.x == vecId.x && n.Position.y == vecId.y).FirstOrDefault();
    }

    private List<BaseNode> CompleteMesh()
    {
        var res = new List<BaseNode>();
        for (int i = 0; i < maxWidth; i++)
            for (int j = 0; j < maxHeight; j++)
                res.Add(new BaseNode(new Vector2Int(i, j)));
        return res;
    }
    private int CalculateDistanceCost(BaseNode a, BaseNode b)
    {
        int xDist = Mathf.Abs(a.Position.x - b.Position.x);
        int yDist = Mathf.Abs(a.Position.y - b.Position.y);
        int rest = Mathf.Abs(xDist - yDist);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDist, yDist) + MOVE_STRAIGHT_COST * rest;
    }
}
