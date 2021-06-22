using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PhysicsMonitor : MonoBehaviour
{
    public Vector2Int endPosition = new Vector2Int();
    public RectInt map = new RectInt();
    public List<Vector2Int> nonWalkablePositions => obstacleListPosition.Select(o => (Vector2Int)o.Position).ToList();
    public List<BaseObstacle> obstacleListPosition = new List<BaseObstacle>();


}
