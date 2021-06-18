using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFallowScript : MonoBehaviour
{
    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        if (target == null)
        {
            target = new GameObject();
            // throw new NullReferenceException("Target must def");
        }
        Update();
    }

    // Update is called once per frame
    void Update()
    {
        // set camera position
        this.transform.position = target.transform.position + new Vector3Int(0, 0, -30);
    }
}
