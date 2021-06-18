using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private MovementControls controls;
    void Start()
    {
        controls = this.gameObject.GetComponent<MovementControls>();

    }

    // Update is called once per frame
    void Update()
    {
        controls.Move(1 * Time.deltaTime, 2 * Time.deltaTime);

    }
}
