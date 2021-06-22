using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DebugerNode : MonoBehaviour
{
    public Vector2Int tilePosition;
    public List<SpriteRenderer> marks = new List<SpriteRenderer>();
    public List<bool> marksBool;

    void OnBecameVisible()
    {
        // Dectivate();
    }
    public void OnEnable()
    {
        // Dectivate();
    }
    public void OnStart()
    {

    }

    public void Dectivate()
    {
        this.gameObject.SetActive(false);
        for (int i = 0; i < marks.Count; i++)
        {
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
        // Debug.Log($"Set node {LayerNumber}");
        if (LayerNumber >= 0 && LayerNumber < marks.Count)
        {
            // Debug.Log($"Set node {LayerNumber} {marks.Count} ");
            this.gameObject.SetActive(true);
            marks[LayerNumber].enabled = true;
            marks[LayerNumber].color = currentPositionColor;
            marksBool[LayerNumber] = true;
        }
    }

    internal bool IsLayerActive(int layerNumber)
    {
        if (layerNumber >= 0 && layerNumber < marks.Count)
            return marksBool[layerNumber];
        return false;
    }
}
