using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class KretanjePoPutanji : MonoBehaviour
{
    public Vector2Int CurrentTilePosition;
    private Rigidbody2D rb;
    private Vector3 velocity = Vector3.zero;
    // private float smoothTime = 1.02f;//.05f;

    List<Vector2Int> currentPath = new List<Vector2Int>();
    Vector3 nextPosition = new Vector3();

    public bool walking = false;
    public Tilemap tmap;
    public float WALK_SPEED = 1.25f;
    public float RUN_SPEED = 2f;
    public float speed = 0f;

    // Start is called before the first frame update
    void Start()
    {
        speed = WALK_SPEED;
        // rb = this.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        UpdateMove();
    }

    private void UpdateMove()
    {
        if (!walking)
            return;

        var distance = Vector3.Distance(nextPosition, this.transform.position);

        if (distance < .05f)
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

        // Use rigid body for moving object
        this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, nextPosition, speed * Time.deltaTime);
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
        // Debug.Log("Update path - " + DateTime.Now.ToString());

        if (path != null && path.Count > 0)
        {
            // Debug.Log($"Update path unutar petlje {path.Count} - " + DateTime.Now.ToString());
            currentPath = path;
            getNextWayPoint();
        }
    }

    private void getNextWayPoint()
    {
        // Debug.Log("get next way point");

        CurrentTilePosition = currentPath[0];
        // nextPosition = tmap.CellToWorld((Vector3Int)CurrentTilePosition);
        nextPosition = tmap.GetCellCenterWorld((Vector3Int)CurrentTilePosition);
        currentPath.RemoveAt(0);
        walking = true;
    }

    internal void Stop()
    {
        walking = false;
    }
}
