using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementControls : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector3 velocity = Vector3.zero;
    [Range(0, .3f)] [SerializeField] private float smoothTime = .02f;//.05f;


    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    public void Move(float moveX, float moveY)
    {

        // Move the character by finding the target velocity
        // Vector3 targetVelocity = new Vector2(moveX * 10f, rb.velocity.y);
        Vector3 targetVelocity = new Vector2(moveX * 10f, moveY * 10f);
        // And then smoothing it out and applying it to the character
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, smoothTime);


    }
}
