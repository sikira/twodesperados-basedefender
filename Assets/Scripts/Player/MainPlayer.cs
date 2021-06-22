using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayer : MonoBehaviour, IHittableObject, IFakeTriggerComponent
{
    public AttackSettings settings = new AttackSettings()
    {
        hittingPower = 51,
        HitRate = .1f
    };
    private MovementControls controls;
    private float horizontalMove = 0;
    private float verticalMove = 0;
    public float speed = 22;

    private int _health = 100;
    public int Health { get => _health; private set => _health = value; }

    public bool alive = true;
    IHittableObject attackTarget;

    public HealtBar healtBar;
    private int startHealth = 100;

    void Start()
    {
        controls = this.GetComponent<MovementControls>();
    }
    void Update()
    {
        horizontalMove = Input.GetAxis("Horizontal") * speed;
        verticalMove = Input.GetAxis("Vertical") * speed;
        settings.currentHitRate += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            MakeAttack();
        }

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
        healtBar?.SetPercent(Health,startHealth);
        // Debug.Log($"Auuuuch {Health} -  {alive}");
    }

    private void Killed()
    {
        alive = false;
        controls.Stop();
        GetComponent<CircleCollider2D>().enabled = false;

        GameObject.FindObjectOfType<UiControl>()?.GameOver();
    }

    public void HealMe(int power)
    {
        Health += power;
        if (Health > 100)
            Health = 100;

        healtBar?.SetPercent(Health , startHealth);
    }

    private void MakeAttack()
    {
        if (settings.currentHitRate > settings.HitRate)
        {
            settings.currentHitRate = 0;
            attackTarget?.HitMe(settings.hittingPower);
        }
    }
    public void ObjectEntered(GameObject obj, int triggerId)
    {
        if (triggerId == 1)
            attackTarget = obj.GetComponent<IHittableObject>();
    }

    public void ObjectExited(GameObject obj, int triggerId)
    {
        if (triggerId == 1)
            attackTarget = null;
    }

    public void ObjectStay(GameObject obj, int triggerId)
    {
        if (triggerId == 1)
            attackTarget = obj.GetComponent<IHittableObject>();
    }
}
