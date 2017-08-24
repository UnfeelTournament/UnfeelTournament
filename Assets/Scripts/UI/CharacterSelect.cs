using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterSelect : MonoBehaviour {

    public Button nextButton;
    public Button backButton;

    public Button[] playerButton;

    public bool[] buttonSelected;

    public int counter = 1;


    public Text whoseTurnIsIt;
    public Text[] playerPicked;

    public bool allPicked = false;

	// Use this for initialization
	void Start () {
        Button b = nextButton.GetComponent<Button>();
        b.onClick.AddListener(onClickSave);
        b.interactable = false;
        Button c = backButton.GetComponent<Button>();
        c.onClick.AddListener(onClickReset);

        buttonSelected = new bool[playerButton.Length];

        for (int i = 0; i < playerButton.Length; i++)
        {
            buttonSelected[i] = false;
        }

        Button h = playerButton[0].GetComponent<Button>();
        h.onClick.AddListener(onClickHappy);
        Button s = playerButton[1].GetComponent<Button>();
        s.onClick.AddListener(onClickSad);
        Button a = playerButton[2].GetComponent<Button>();
        a.onClick.AddListener(onClickAngry);
        Button w = playerButton[3].GetComponent<Button>();
        w.onClick.AddListener(onClickWorried);

    }
	
	// Update is called once per frame
	void Update () {
        if (counter <= GlobalManager._instance._numOfPlayers && !allPicked) whoseTurnIsIt.text = "Player " + counter + "'s turn to pick!.";
        else
        {
            whoseTurnIsIt.text = "All Players picked!";
            nextButton.interactable = true;
        }
        
	}

    public void onClickSave()
    {
        if (counter <= GlobalManager._instance._numOfPlayers) return;
        allPicked = true;
        //Nothing so far
        //Save each player's choice (button chocie) to Global Manager playerType
        //GlobalManager._instance._playerTypes = types;
        counter = 0;
    }

    public void onClickReset()
    {
        for(int i = 0; i < buttonSelected.Length; i++)
        {
            buttonSelected[i] = false;
            playerPicked[i].gameObject.SetActive(false);
        }
        counter = 1;
        nextButton.interactable = false;
        allPicked = false;
    }

    public void onClickHappy()
    {
        onClickCharacter(0, counter - 1);
    }
    public void onClickSad()
    {
        onClickCharacter(1, counter - 1);
    }
    public void onClickWorried()
    {
        onClickCharacter(3, counter - 1);
    }
    public void onClickAngry()
    {
        onClickCharacter(2, counter - 1);
    }
    public void onClickCharacter(int type, int player)
    {
        //Debug.Log("Type: "+type);
        //Debug.Log("Player/Counter: " + player);
        if(buttonSelected[type] || counter > GlobalManager._instance._numOfPlayers || allPicked)
        {
            return;
        }
        GlobalManager._instance._playerTypes[player] = type;
        counter++;
        playerPicked[type].gameObject.SetActive(true);
        buttonSelected[type] = true;
    }
}
