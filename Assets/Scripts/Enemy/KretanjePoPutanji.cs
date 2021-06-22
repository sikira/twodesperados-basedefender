using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class KretanjePoPutanji : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector3 velocity = Vector3.zero;
    [Range(0, .3f)] [SerializeField] private float smoothTime = .02f;//.05f;

    List<Vector2Int> currentPath = new List<Vector2Int>();
    Vector3 nextPosition = new Vector3();

    public bool walking = false;
    public Tilemap tmap;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        UpdateMove();

    }

    private void UpdateMove()
    {
        if (!walking)
            return;

        var distance = Vector3.Distance(nextPosition, rb.position);

        if (distance < .5f)
        {
            if (currentPath.Count > 0)
            {
                getNextWayPoint();
            }
            else
            {
                walking = false;
            }
        }

        // var distance = Vector3.Distance(nextPosition, rb.position);
        // Vector3 targetVelocity = new Vector2(moveX * 10f, moveY * 10f);

        rb.velocity = Vector3.SmoothDamp(rb.velocity, nextPosition, ref velocity, smoothTime);

    }

    // Update is called once per frame
    // public void Move(float moveX, float moveY)
    // {

    //     // Move the character by finding the target velocity
    //     // Vector3 targetVelocity = new Vector2(moveX * 10f, rb.velocity.y);
    //     // Vector3 targetVelocity = new Vector2(moveX * 10f, moveY * 10f);
    //     // // And then smoothing it out and applying it to the character
    //     // rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, smoothTime);
    // }

    internal void UpdatePath(List<Vector2Int> path)
    {
        if (path != null && path.Count > 0)
        {
            currentPath = path;
            getNextWayPoint();
        }
    }

    private void getNextWayPoint()
    {
        var tilePos = currentPath[0];
        nextPosition = tmap.CellToWorld((Vector3Int)tilePos);
        currentPath.RemoveAt(0);
        walking = true;
    }
}
