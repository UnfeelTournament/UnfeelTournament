using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

/* Maybe have a public Button for every button and put the BeenClicked code into the other functions players1..."*/
/*IDEA: - MAYBE
-Have a public Button for each Button
-In OnEnable(), for each button in the array, place the listener function for it using AddListener (delegate {function})
*/
//https://www.reddit.com/r/Unity3D/comments/2z477c/how_do_you_guys_detect_if_a_ui_button_was_pressed/
public class GameStartSettings : MonoBehaviour {

	static public int livesOrTime = 1;
	static public int players = 2;
	static public float time;
	static public string gameMode = "Stock";
	public Button finishButton;
    public GameObject[] playerButtons;
    public GameObject[] livesButtons;

	public void Start()
	{
		for (int i = 0; i < GetComponentsInChildren<UnityEngine.UI.Text>().Length; i++)
		{
			if (GetComponentsInChildren<UnityEngine.UI.Text>()[i].text == "me")
			{
				Debug.Log(i);
			}
		}

        Button b = finishButton.GetComponent<Button>();
        b.onClick.AddListener(onClickSave);

    }

	public void Update()
	{
		if (buttonSelected.isSelectedHeart && buttonSelected.isSelectedLife){
			if(parseTime(time) != "0:00"){
					foreach (GameObject finish in GameObject.FindGameObjectsWithTag("Finish"))
					{
						finish.GetComponent<Button>().interactable = true;
					}
				}

		}
        for(int i = 0; i < livesButtons.Length; i++)
        {
            //Debug.Log("LIVES I: " + i);
            if (livesButtons[i].GetComponent<buttonSelected>().ButtonOn)
            {
                //Debug.Log("LIFE BUTTON ON: " + i);
                onClickLives(i);
            }
        }
        for(int i = 0; i < playerButtons.Length; i++)
        {
            //Debug.Log("PLAYERS I: " + i);
            if (playerButtons[i].GetComponent<buttonSelected>().ButtonOn)
            {
                //Debug.Log("PLAYER BUTTON ON: " + i);
                onClickPlayer(i + 1);
            }
        }
	}

    public void onClickSave()
    {
        //Debug.Log("Finish Button was clicked.");
        saveSettings();
        //Save level
    }

    public void saveSettings()
    {
        //Debug.Log("saving settings");
        //Debug.Log("saving settings: " + players);
        GlobalManager._instance._numOfLives = livesOrTime;
        GlobalManager._instance._numOfPlayers = players;
        //Debug.Log("TIME: " + time);
        GlobalManager._instance._gameTime = time;
        //Temporary
        GlobalManager._instance._gameStage = 0;
        GlobalManager._instance._useTime = time == 0f ? false : true;
        GlobalManager._instance.initializePlayerArrays();
    }

	public void timeSelect()
	{
		//GetComponent<AudioSource>().Play();
		timeSet();
		gameMode = "Timer";
		GetComponentsInChildren<UnityEngine.UI.Text>()[17].text = "Timer";
	}

/*	public void stockSelect()
	{
		GetComponent<AudioSource>().Play();
		gameMode = "Stock";
		GetComponentsInChildren<UnityEngine.UI.Text>()[6].text = "Stock";
	}
*/

	public void timeSet()
	{
		time = GetComponentInChildren<UnityEngine.UI.Slider>().value * 60;
		GetComponentsInChildren<UnityEngine.UI.Text>()[18].text = parseTime(time);
	}

	private string parseTime(float f)
	{
        //Debug.Log("IN PARSE TIME");
		String secondsString;
		int minutes =  (int) f/60;
		int seconds = (int)(((f/60) - minutes) * 60);

		secondsString = seconds < 10 ? "0" + seconds.ToString() : seconds.ToString();

		return minutes + ":" + secondsString;
	}

    public void onClickPlayer(int i)
    {
        players = i + 1;
        //SoundController.clip = select;
        //SoundController.Play();
    }
    public void onClickLives(int i)
    {
        livesOrTime = i + 1;
        //SoundController.clip = select;
       // SoundController.Play();
    }
    //**************************
    //Players buttons
    //**************************


    public void players1()
	{
		//GetComponent<AudioSource>().Play();
		GetComponentsInChildren<UnityEngine.UI.Text>()[14].text = "1";
        Debug.Log("Players 1 CLICKED");
		players = 1;
	}
	public void players2()
	{
		//GetComponent<AudioSource>().Play();
		GetComponentsInChildren<UnityEngine.UI.Text>()[16].text = "2";
        Debug.Log("Players 2 CLICKED");
        players = 2;
	}
	public void players3()
	{
		//GetComponent<AudioSource>().Play();
		GetComponentsInChildren<UnityEngine.UI.Text>()[17].text = "3";
        Debug.Log("Players 3 CLICKED");
        players = 3;
	}
	public void players4()
	{
		//GetComponent<AudioSource>().Play();
		GetComponentsInChildren<UnityEngine.UI.Text>()[15].text = "4";
        Debug.Log("Players 4 CLICKED");
        players = 4;
	}

	//**************************
	//Lives buttons
	//**************************

	public void click1()
	{
		//GetComponent<AudioSource>().Play();
		GetComponentsInChildren<UnityEngine.UI.Text>()[3].text = "1";
		livesOrTime = 1;
	}
	public void click2()
	{
		//GetComponent<AudioSource>().Play();
		GetComponentsInChildren<UnityEngine.UI.Text>()[4].text = "2";
		livesOrTime = 2;
	}
	public void click3()
	{
		//GetComponent<AudioSource>().Play();
		GetComponentsInChildren<UnityEngine.UI.Text>()[5].text = "3";
		livesOrTime = 3;
	}
	public void click4()
	{
		//GetComponent<AudioSource>().Play();
		GetComponentsInChildren<UnityEngine.UI.Text>()[6].text = "4";
		livesOrTime = 4;
	}
	public void click5()
	{
		//GetComponent<AudioSource>().Play();
		GetComponentsInChildren<UnityEngine.UI.Text>()[7].text = "5";
		livesOrTime = 5;
	}
	public void click6()
	{
		//GetComponent<AudioSource>().Play();
		GetComponentsInChildren<UnityEngine.UI.Text>()[8].text = "6";
		livesOrTime = 6;
	}
	public void click7()
	{
		//GetComponent<AudioSource>().Play();
		GetComponentsInChildren<UnityEngine.UI.Text>()[9].text = "7";
		livesOrTime = 7;
	}
	public void click8()
	{
		//GetComponent<AudioSource>().Play();
		GetComponentsInChildren<UnityEngine.UI.Text>()[10].text = "8";
		livesOrTime = 8;
	}
	public void click9()
	{
		//GetComponent<AudioSource>().Play();
		GetComponentsInChildren<UnityEngine.UI.Text>()[11].text = "9";
		livesOrTime = 9;
	}
		
}
