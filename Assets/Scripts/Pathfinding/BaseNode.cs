using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseNode
{
    // Vrijednost od pocetka
    public int G { get; set; } = int.MaxValue;
    // Pretpostavljena Vrijednost do cilja
    public int H { get; set; } = 0;
    // Ukupna cijena 
    public int F => H + G;
    public Vector2Int Position = Vector2Int.zero;
    public BaseNode Parent;

    public BaseNode(Vector2Int Position)
    {
        this.Position = Position;
    }

    public override string ToString() => $"BaseNode({ Position.x}, { Position.y} )";
    public static bool operator ==(BaseNode left, BaseNode right)
    {
        if (left?.Position.x == right?.Position.x && left?.Position.y == right?.Position.y)
            return true;
        return false;
    }
    public static bool operator !=(BaseNode left, BaseNode right) => !(left == right);

    public bool Equals(BaseNode right) => this == right;

    public override int GetHashCode() => Position.GetHashCode();

    //TODO: sredi ovo jer si hardovvao da ne izbaljue gresku
    public bool Equals(Object right) => false;
}
