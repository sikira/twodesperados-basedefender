using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiControl : MonoBehaviour
{

    public Canvas succes;
    public Canvas fail;
    // Start is called before the first frame update
    void Start()
    {
        fail.enabled = false;
        succes.enabled = false;

        this.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

    }

    internal void PlayerKilled()
    {
        Time.timeScale = .05f;
        fail.enabled = true;
    }
    internal void PlayerWon()
    {
        Time.timeScale = .2f;
        succes.enabled = true;
    }
}
