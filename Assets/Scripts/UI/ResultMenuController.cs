using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ResultMenuController : MonoBehaviour {
    public GameObject TWOP;
    public GameObject THREEP;
    public GameObject FOURP;

    public Text TWOP_Winner_Text;
    public Text THREEP_Winner_Text;
    public Text FOURP_Winner_Text;

    public Text[] TWOP_Player_Text;
    public Text[] THREEP_Player_Text;
    public Text[] FOURP_Player_Text;

    public Text[] TWOP_Kills_Text;
    public Text[] THREEP_Kills_Text;
    public Text[] FOURP_Kills_Text;

    public Text[] TWOP_Deaths_Text;
    public Text[] THREEP_Deaths_Text;
    public Text[] FOURP_Deaths_Text;

    public Text[] TWOP_Damage_Dealt_Text;
    public Text[] THREEP_Damage_Dealt_Text;
    public Text[] FOURP_Damage_Dealt_Text;

    public Text[] TWOP_Damage_Taken_Text;
    public Text[] THREEP_Damage_Taken_Text;
    public Text[] FOURP_Damage_Taken_Text;

    public int PlayerNumber;
    public CharacterData[] Logs;

    public Button next;

    // Use this for initialization
    void Start () {
        Button b = next.GetComponent<Button>();
        b.onClick.AddListener(onClickNext);

        PlayerNumber = GlobalManager._instance._numOfPlayers;
        Logs = new CharacterData[PlayerNumber];
        for(int i = 0; i < PlayerNumber; i++)
        {
            Logs[i] = GlobalManager._instance._characterLogs[i];
        }
        switch (PlayerNumber)
        {
            case 2:
                TWOP.SetActive(true);
                break;
            case 3:
                THREEP.SetActive(true);
                break;
            case 4:
                FOURP.SetActive(true);
                break;
            default:
                Debug.Log("ErrorS");
                break;
        }

        setLogs();
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void onClickNext()
    {
        //Debug.Log("clicked");
        GlobalManager._instance._resultToPost = true;
    }

    public void setLogs()
    {
        switch (PlayerNumber)
        {
            case 2:
                log2P();
                break;
            case 3:
                log3P();
                break;
            case 4:
                log4P();
                break;
            default:
                Debug.Log("Error");
                break;
        }
    }

    public void setWinner(Text txt)
    {
        int winner = GlobalManager._instance._winner;
        if (winner != -1 && winner != -2)
        {
            int type = GlobalManager._instance._playerTypes[winner];
            txt.text = getName(type);
            txt.color = getColor(type);
        }
        else
        {
            txt.text = "No One!";
            txt.color = Color.white;
        }
    }

    public void log2P()
    {
        for(int i = 0; i < PlayerNumber; i++)
        {
            //Set Players
            TWOP_Player_Text[i].text = getName(GlobalManager._instance._playerTypes[i]);
            TWOP_Player_Text[i].color = getColor(GlobalManager._instance._playerTypes[i]);
            //Set Kills
            TWOP_Kills_Text[i].text = "" + Logs[i]._kills;
            //Set Deaths
            TWOP_Deaths_Text[i].text = "" + Logs[i]._deaths;
            //Set Damage Dealt
            TWOP_Damage_Dealt_Text[i].text = "" + Logs[i]._damageDealt;
            //Set Damage Taken
            TWOP_Damage_Taken_Text[i].text = "" + Logs[i]._damageTaken;
        }
        setWinner(TWOP_Winner_Text);
    }

    public void log3P()
    {
        for (int i = 0; i < PlayerNumber; i++)
        {
            //Set Players
            THREEP_Player_Text[i].text = getName(GlobalManager._instance._playerTypes[i]);
            THREEP_Player_Text[i].color = getColor(GlobalManager._instance._playerTypes[i]);
            //Set Kills
            THREEP_Kills_Text[i].text = "" + Logs[i]._kills;
            //Set Deaths
            THREEP_Deaths_Text[i].text = "" + Logs[i]._deaths;
            //Set Damage Dealt
            THREEP_Damage_Dealt_Text[i].text = "" + Logs[i]._damageDealt;
            //Set Damage Taken
            THREEP_Damage_Taken_Text[i].text = "" + Logs[i]._damageTaken;
        }
        setWinner(THREEP_Winner_Text);
    }

    public void log4P()
    {
        for (int i = 0; i < PlayerNumber; i++)
        {
            //Set Players
            FOURP_Player_Text[i].text = getName(GlobalManager._instance._playerTypes[i]);
            FOURP_Player_Text[i].color = getColor(GlobalManager._instance._playerTypes[i]);
            //Set Kills
            FOURP_Kills_Text[i].text = "" + Logs[i]._kills;
            //Set Deaths
            FOURP_Deaths_Text[i].text = "" + Logs[i]._deaths;
            //Set Damage Dealt
            FOURP_Damage_Dealt_Text[i].text = "" + Logs[i]._damageDealt;
            //Set Damage Taken
            FOURP_Damage_Taken_Text[i].text = "" + Logs[i]._damageTaken;
        }
        setWinner(FOURP_Winner_Text);
    }

    public string getName(int type)
    {
        switch (type)
        {
            case 0:
                return "Happy";
            case 1:
                return "Sad";
            case 2:
                return "Angry";
            case 3:
                return "Worried";
            default:
                return "Error";
        }
    }

    public Color getColor(int type)
    {
        switch (type)
        {
            case 0:
                return Color.yellow;
            case 1:
                return Color.blue;
            case 2:
                return Color.red;
            case 3:
                return Color.green;
            default:
                return Color.white;
        }
    }
}
