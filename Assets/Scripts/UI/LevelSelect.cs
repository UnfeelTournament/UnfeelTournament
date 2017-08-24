using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelSelect : MonoBehaviour {
    public Button playButton;
    public Button backButton;

    public Button[] levelButtons;

    public int _selectedLevel = -1;
	// Use this for initialization
	void Start () {

        Button b = playButton.GetComponent<Button>();
        b.onClick.AddListener(onClickSave);
        b.interactable = false;
        Button c = backButton.GetComponent<Button>();
        c.onClick.AddListener(onClickReset);

        Button l0 = levelButtons[0].GetComponent<Button>();
        l0.onClick.AddListener(onClickLevel0);
        Button l1 = levelButtons[1].GetComponent<Button>();
        l1.onClick.AddListener(onClickLevel1);
        Button l2 = levelButtons[2].GetComponent<Button>();
        l2.onClick.AddListener(onClickLevel2);
        Button l3 = levelButtons[3].GetComponent<Button>();
        l3.onClick.AddListener(onClickLevel3);
    }
	
	// Update is called once per frame
	void Update () {
        if (_selectedLevel != -1) playButton.interactable = true;
	}

    public void onClickLevel0()
    {
        onClickLevel(0);
    }
    public void onClickLevel1()
    {
        onClickLevel(1);
    }
    public void onClickLevel2()
    {
        onClickLevel(2);
    }
    public void onClickLevel3()
    {
        onClickLevel(3);
    }

    public void onClickReset()
    {
        _selectedLevel = -1;
        playButton.interactable = false;
    }

    public void onClickSave()
    {
        GlobalManager._instance._gameStage = _selectedLevel; //_selectedLevel
        GlobalManager._instance._mainToGame = true;
    }

    public void onClickLevel(int l)
    {
        _selectedLevel = l;
    }
}
