using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControls : MonoBehaviour
{

    public float mm;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(Input.GetMouseButtonDown(0));


        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                Debug.Log(touch);

                // Construct a ray from the current touch coordinates
                // Ray ray = Camera.main.ScreenPointToRay(touch.position);
                // if (Physics.Raycast(ray))
                // {
                //     // Create a particle if hit
                //     // Instantiate(particle, transform.position, transform.rotation);
                // }
            }
        }
    }
}
