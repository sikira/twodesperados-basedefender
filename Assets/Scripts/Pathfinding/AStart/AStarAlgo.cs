using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AStarAlgo : INodePathfinder
{

    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;
    int maxWidth;
    int maxHeight;
    private List<BaseNode> openList;
    private List<BaseNode> closedList;
    private List<BaseNode> completeMap;

    public void Find(Vector2Int startPosition, Vector2Int endPosition, LevelData dataFake)
    {
        maxWidth = dataFake.SizeX;
        maxHeight = dataFake.SizeY;

        openList = new List<BaseNode>();
        closedList = new List<BaseNode>();
        completeMap = CompleteMesh();

        var endNode = new BaseNode(startPosition);

        var startNode = new BaseNode(startPosition);
        startNode.G = 0;
        startNode.H = CalculateDistanceCost(startNode, endNode);


        openList.Add(startNode);



        while (openList.Count > 0)
        {
            BaseNode lowestFCostNode = openList.OrderBy(n => n.F).First();

            if (lowestFCostNode == endNode)
            {
                Debug.Log("nasli smo nesto");
                return;
            }

            openList.Remove(lowestFCostNode);
            closedList.Add(lowestFCostNode);

            // convert to IEnumeralble and use in foreach
            var neighbourList = GetNeighbourList(lowestFCostNode);

            foreach (var neighnourNode in neighbourList)
            {
                
            }

        }
    }

    private List<BaseNode> GetNeighbourList(BaseNode currentNode)
    {
        var neighbourList = new List<BaseNode>();
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                BaseNode goodNode = GetNode(currentNode.Position + new Vector2Int(i, j));
                if (goodNode != null)
                    neighbourList.Add(goodNode);
            }
        }
        return neighbourList;
    }

    private BaseNode GetNode(Vector2Int vecId)
    {
        // TODO: calculate obstacles

        // check if allready is in closed list
        if (closedList.Where(n => n.Position.x == vecId.x && n.Position.y == vecId.y).Count() > 0)
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
