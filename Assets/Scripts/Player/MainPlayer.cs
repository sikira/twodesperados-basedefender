using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayer : MonoBehaviour
{
    private PlayersControls controls;
    private float horizontalMove = 0;
    private float verticalMove = 0;
    public float speed = 22;

    void Start()
    {
        controls = this.GetComponent<PlayersControls>();


    }
    void Update()
    {
        horizontalMove = Input.GetAxis("Horizontal") * speed;
        verticalMove = Input.GetAxis("Vertical") * speed;


    }
    void FixedUpdate()
    {
        // controls
        controls.Move(horizontalMove * Time.deltaTime, verticalMove * Time.deltaTime);


    }


}
