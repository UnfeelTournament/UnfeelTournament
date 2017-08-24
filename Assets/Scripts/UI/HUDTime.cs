using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDTime : MonoBehaviour {
    public Text _time;
    public GameObject _pause;
    int level;
    // Use this for initialization
    void Start () {
        _time.text = parseTime(GlobalManager._instance._gameTime);
        level = GlobalManager._instance._gameStage;
    }
	
	// Update is called once per frame
	void Update () {
        float currentTime = GlobalManager._instance._currentTime;

        if (currentTime >= 0)
        {
            _time.text = parseTime(currentTime);
            _time.color = setColor(level);
        }
        else
        {
            _time.text = parseTime(0f);
            _time.color = setColor(level);
        }
    }

    public string parseTime(float time)
    {
        return string.Format("{0}:{1:00}", (int)time / 60, (int)time % 60);
    }

    public void togglePause(bool toggle)
    {
        //Debug.Log("toggle Paused: " + toggle);
        _pause.SetActive(toggle);
    }

    public Color setColor(int level)
    {
        switch (level)
        {
            case 0:
                return Color.black;
            case 1:
                return Color.grey;
            case 2:
                return Color.black;
            case 3:
                return Color.white;
            default:
                return Color.cyan;
        }
    }
}
