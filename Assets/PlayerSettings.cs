using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSettings : MonoBehaviour
{
    // Start is called before the first frame update
    public bool MusicOn
    {
        get => PlayerPrefs.GetInt("Music", 0) == 1;
        set
        {
            var newValue = value ? 1 : 0;
            PlayerPrefs.SetInt("Music", newValue);
        }
    }

    public bool SoundOn
    {
        get => PlayerPrefs.GetInt("SoundOn", 0) == 1;
        set
        {
            var newValue = value ? 1 : 0;
            PlayerPrefs.SetInt("SoundOn", newValue);
        }
    }
    public bool DebuggerOn
    {
        get => PlayerPrefs.GetInt("DebuggerOn", 1) == 1;
        set
        {
            var newValue = value ? 1 : 0;
            PlayerPrefs.SetInt("DebuggerOn", newValue);
        }
    }

    public string gridX
    {
        get => PlayerPrefs.GetString("gridX", "30");
        set => PlayerPrefs.SetString("gridX", value);
    }

    public string gridY
    {
        get => PlayerPrefs.GetString("gridY", "20");
        set => PlayerPrefs.SetString("gridY", value);
    }

    public string numOfObstacles
    {
        get => PlayerPrefs.GetString("numOfObstacles", "30");
        set => PlayerPrefs.SetString("numOfObstacles", value);
    }

    public string currentLevel
    {
        get => PlayerPrefs.GetString("currentLevel", "1");
        set => PlayerPrefs.SetString("currentLevel", value);
    }




}
