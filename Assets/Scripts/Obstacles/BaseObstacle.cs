using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// public class BaseObstacle : MonoBehaviour
public class BaseObstacle : MonoBehaviour, IHittableObject
{
    public Vector2Int Position;

    public BaseObstacle(Vector2Int position)
    {
        Position = position;
    }

    private int _health = 150;
    public int Health => _health;

    public void HealMe(int power)
    {
        _health += power;
    }

    public void HitMe(int power)
    {
        _health -= power;
        if (_health <= 0)
        {
            DestroyMe();
        }
    }

    private void DestroyMe()
    {
        this.gameObject.SetActive(false);
        
        //TODO: Obavjesti sve da se promjenila fizika

    }
}
