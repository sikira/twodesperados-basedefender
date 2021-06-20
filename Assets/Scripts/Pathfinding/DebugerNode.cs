using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DebugerNode : MonoBehaviour
{
    public List<SpriteRenderer> marks = new List<SpriteRenderer>();
    public List<bool> marksBool = new List<bool>();

    void OnBecameVisible()
    {
        Dectivate();
    }
    public void OnEnable()
    {
        Dectivate();
    }

    public void Dectivate()
    {
        this.gameObject.SetActive(false);
        marksBool = new List<bool>();
        for (int i = 0; i < marks.Count; i++)
        {
            marksBool.Add(false);
            DeactivateMark(i);
        }
    }

    public void DeactivateMark(int LayerNumber)
    {
        marks[LayerNumber].enabled = false;
        marks[LayerNumber].color = Color.white;
        marksBool[LayerNumber] = false;

        if (!marksBool.Contains(true))
            this.gameObject.SetActive(false);
    }



    public void SetNode(int LayerNumber, Color currentPositionColor)
    {
        if (LayerNumber > 0 && LayerNumber < marks.Count)
        {
            this.gameObject.SetActive(true);
            marks[LayerNumber].enabled = true;
            marks[LayerNumber].color = currentPositionColor;
            marksBool[LayerNumber] = true;
        }
    }

    internal bool IsLayerActive(int layerNumber)
    {
        if (layerNumber > 0 && layerNumber < marks.Count)
            return marksBool[layerNumber];
        return false;
    }
}
