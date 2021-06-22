using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private int fingerId = -1;
    private Vector2 startPosition;

    void Start()
    {
        controls = this.GetComponent<MovementControls>();
    }

    public void ControlerMoveStarted()
    {

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

        if (Input.touchCount > 0)
        {
            // var touch = Input.touches.Where(t => t.phase == TouchPhase.Began).FirstOrDefault();

            foreach (var touch in Input.touches)
            {


                if (touch.phase == TouchPhase.Began && touch.position.x > 0 && touch.position.y > 0
                && touch.position.x < 300 && touch.position.y < 300 && fingerId < 0)
                {
                    fingerId = touch.fingerId;
                    startPosition = touch.position;

                }
                else if (touch.phase == TouchPhase.Ended && touch.fingerId == fingerId)
                {
                    fingerId = -1;
                }
                else if (touch.fingerId == fingerId)
                {
                    var dist = Vector2.Distance(startPosition, touch.position);

                    if (dist > 15f || touch.deltaPosition.magnitude > 5)
                    {
                        var resVec = touch.position - startPosition;
                        var horSirX = resVec.x < 0 ? -1 : resVec.x > 0 ? 1 : 0;
                        var horSirY = resVec.y < 0 ? -1 : resVec.y > 0 ? 1 : 0;
                        // var horSirX = touch.deltaPosition.x < 0 ? -1 : touch.deltaPosition.x > 0 ? 1 : 0;
                        // var horSirY = touch.deltaPosition.y < 0 ? -1 : touch.deltaPosition.y > 0 ? 1 : 0;
                        horizontalMove = horSirX * speed * .75f;
                        verticalMove = horSirY * speed * .75f;
                    }
                }

                // Debug.Log($"Touch: x:{touch.position.x} , y:{touch.position.y} , id:{touch.fingerId },    ");
            }

        }
        else
        {
            fingerId = -1;
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
        healtBar?.SetPercent(Health, startHealth);
        // Debug.Log($"Auuuuch {Health} -  {alive}");
    }

    private void Killed()
    {
        alive = false;
        controls.Stop();
        GetComponent<CircleCollider2D>().enabled = false;

        GameObject.FindObjectOfType<UiControl>()?.GameOver("Player died");
    }

    public void HealMe(int power)
    {
        Health += power;
        if (Health > 100)
            Health = 100;

        healtBar?.SetPercent(Health, startHealth);
    }


    public void MakeAttack()
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
