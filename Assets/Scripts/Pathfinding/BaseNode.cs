using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseNode : MonoBehaviour
{
    // Vrijednost od pocetka
    public int G { get; set; } = 0;
    // Pretpostavljena Vrijednost do cilja
    public int H { get; set; } = 0;
    // Ukupna cijena 
    public int F => H + G;
    public Vector3Int Position;
    public BaseNode Parent;

    public BaseNode(Vector3Int Position)
    {
        this.Position = Position;
    }
}
