using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour, IHittableObject
{
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
            GameOver();
        }
    }

    private void GameOver()
    {
        DestroyMe();
        GameObject.FindObjectOfType<UiControl>()?.GameOver();
    }

    private void DestroyMe()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        this.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
