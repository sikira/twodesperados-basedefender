using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayer : MonoBehaviour, IHittableObject
{
    private MovementControls controls;
    private float horizontalMove = 0;
    private float verticalMove = 0;
    public float speed = 22;

    private int _health = 100;
    public int Health { get => _health; private set => _health = value; }

    public bool alive = true;

    void Start()
    {
        controls = this.GetComponent<MovementControls>();
    }
    void Update()
    {
        horizontalMove = Input.GetAxis("Horizontal") * speed;
        verticalMove = Input.GetAxis("Vertical") * speed;

    }
    void FixedUpdate()
    {
        if (!alive)
            return;

        controls.Move(horizontalMove * Time.deltaTime, verticalMove * Time.deltaTime);
    }

    public void HitMe(int power)
    {

        Health -= power;
        if (Health <= 0)
        {
            Killed();
        }
        Debug.Log($"Auuuuch {Health} -  {alive}");
    }

    private void Killed()
    {
        alive = false;
        controls.Stop();
        GetComponent<CircleCollider2D>().enabled = false;

        GameObject.FindObjectOfType<UiControl>()?.PlayerKilled();

    }

    public void HealMe(int power)
    {
        Health += power;
        if (Health > 100)
            Health = 100;
    }
}
