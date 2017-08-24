using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WinController : MonoBehaviour {
    public GameObject happyWin;
    public GameObject sadWin;
    public GameObject angryWin;
    public GameObject worriedWin;
    public GameObject nobodyWin;

    public Button toMain;
	// Use this for initialization
	void Start () {
        Button b = toMain.GetComponent<Button>();
        b.onClick.AddListener(onClickToMain);
        int winner = GlobalManager._instance._winner;
        int winnerType = winner >= 0 ? GlobalManager._instance._playerTypes[winner] : -1;
        switch (winnerType)
        {
            case 0:
                happyWin.gameObject.SetActive(true);
                //background.sprite = (HAPPY)
                break;
            case 1:
                sadWin.gameObject.SetActive(true);
                //background.sprite = (SAD)
                break;
            case 2:
                angryWin.gameObject.SetActive(true);
                //background.sprite = (ANGRY)
                break;
            case 3:
                worriedWin.gameObject.SetActive(true);
                //background.sprite = (WORRIED)
                break;
            case -1:
                nobodyWin.gameObject.SetActive(true);
                //background.sprite = (NOBODY)
                break;
            default:
                Debug.Log("Bad Player Type");
                break;
        }
	}
	
    public void onClickToMain()
    {
        GlobalManager._instance._postToMain = true;
    }

	// Update is called once per frame
	void Update () {
	
	}
}
