using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiControl : MonoBehaviour
{
    //BUG: igrac moze proci kroz zidove ako ga guraju

    public Canvas succes;
    public Canvas fail;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        fail.enabled = false;
        succes.enabled = false;

        this.gameObject.SetActive(true);
    }

    // Update is called once per frame
    // internal void LevelPassed()
    // {
    //     Time.timeScale = .05f;
    //     succes.enabled = true;
    // }


    internal void GameOver()
    {
        Time.timeScale = .05f;
        fail.enabled = true;
    }
    internal void PlayerWon()
    {
        // TODO: ubaciti .2f ali prekuniti sve akcije
        Time.timeScale = 0f;
        succes.enabled = true;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ExitGame()
    {
        SceneManager.LoadScene(0);
    }
    public void GoToNextLevel()
    {
        var num  = int.Parse(GameObject.FindObjectOfType<PlayerSettings>().currentLevel);
        num++;
        GameObject.FindObjectOfType<PlayerSettings>().currentLevel = num.ToString();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

}
