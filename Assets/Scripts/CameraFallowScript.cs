using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFallowScript : MonoBehaviour
{
    public GameObject target;
    // Start is called before the first frame update

    [SerializeField] private float smoothTime = 1f;
    [SerializeField] private float maxDistance = 3f;//5f;
    private Vector3 velocity = Vector3.zero;


    private bool isFallowing = true;
    void Start()
    {
        if (target == null)
        {
            target = new GameObject();
            // throw new NullReferenceException("Target must def");
        }

        transform.position = targetPosition;
    }
    private Vector3 targetPosition => target.transform.position + new Vector3(0, 0, -10);
    void Update()
    {
        var distance = Vector3.Distance(this.transform.position, targetPosition);

        if (distance > maxDistance && isFallowing == false)
        {
            isFallowing = true;
        }
        else if (distance < .5f && isFallowing == true)
        {
            isFallowing = false;
        }

        if (isFallowing)
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
