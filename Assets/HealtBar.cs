using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealtBar : MonoBehaviour
{
    public GameObject display;

    public void SetPercent(int start, int max)
    {
        var percent = (float)start / (float)max;
        if (percent < 0)
            percent = 0;
        if (percent > 1)
            percent = 1;
        display.transform.localScale = new Vector3(percent, 1, 1);
    }
}
