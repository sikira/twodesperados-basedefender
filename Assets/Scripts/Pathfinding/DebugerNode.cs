using System;
using System.Collections.Generic;
using UnityEngine;

public class DebugerNode : MonoBehaviour
{
    public List<SpriteRenderer> marks = new List<SpriteRenderer>();

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
        foreach (var mark in marks)
        {
            DeactivateMark(mark);
        }
    }

    private void DeactivateMark(SpriteRenderer mark)
    {
        mark.enabled = false;
        mark.color = Color.white;
    }

    internal void SetNode(Color currentPositionColor)
    {
        this.gameObject.SetActive(true);
        marks[0].enabled = true;
        marks[0].color = currentPositionColor;
    }
}
