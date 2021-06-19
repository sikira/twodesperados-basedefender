using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseNode : MonoBehaviour
{
    // Vrijednost od pocetka
    public int G = 0;
    // Pretpostavljena Vrijednost do cilja
    public int H = 0;    
    // Ukupna cijena 
    public int F => H + G;

    
}
