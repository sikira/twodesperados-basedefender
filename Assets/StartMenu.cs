using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    public PlayerSettings mainSettings;
    public Canvas main;
    public Canvas settings;
    public Canvas background;

    public Toggle soundOn;
    public Toggle musicOn;

    public TMP_InputField sizeX;
    public TMP_InputField sizeY;
    public TMP_InputField num;
    public TextMeshProUGUI level;

    void Start()
    {
        mainSettings = gameObject.GetComponent<PlayerSettings>();
        background.enabled = true;
        main.enabled = true;
        settings.enabled = false;

        soundOn.isOn = mainSettings.SoundOn;
        musicOn.isOn = mainSettings.MusicOn;

        sizeX.text = mainSettings.gridX;
        sizeY.text = mainSettings.gridY;
        num.text = mainSettings.numOfObstacles;
        level.text = "Level " + mainSettings.currentLevel;
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(1);
    }


    public void GoToSettings()
    {
        main.enabled = false;
        settings.enabled = true;
    }

    public void ReturnToMain()
    {
        main.enabled = true;
        settings.enabled = false;
    }
    public void ExitApp()
    {
        //TODO: upit da li zelis :D
        Application.Quit();
    }

}
