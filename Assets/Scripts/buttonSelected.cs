using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class buttonSelected : MonoBehaviour {
	public bool isHeart = false, isLife = false;
	public bool ButtonOn = false;
	public static bool isSelectedHeart = false, isSelectedLife = false;

    //public GameObject SoundController;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//doesn't work, trying to get lives and number of players to stay in a highlighted state when chosen.
	public void BeenClicked()
	{
		if (isHeart) {
			isSelectedHeart = true;
			foreach (GameObject heart in GameObject.FindGameObjectsWithTag("Heart"))
			{
				heart.GetComponent<buttonSelected>().ButtonOn = false;
				heart.GetComponent<Image>().color = Color.white;
			}

			ButtonOn = true;

			if (ButtonOn)
			{
				this.GetComponent<Image>().color = Color.gray;
                //SoundController.GetComponent<SoundController>().setAudioSelect();
			}
			else
			{
				this.GetComponent<Image>().color = Color.white;
			}
		}

		if (isLife) {
			isSelectedLife = true;

			foreach (GameObject life in GameObject.FindGameObjectsWithTag("Life"))
			{
				life.GetComponent<buttonSelected>().ButtonOn = false;
				life.GetComponent<Image>().color = Color.white;
			}

			ButtonOn = true;

			if (ButtonOn)
			{
				this.GetComponent<Image>().color = Color.gray;
                //SoundController.GetComponent<SoundController>().setAudioSelect();
            }
			else
			{
				this.GetComponent<Image>().color = Color.white;
			}
		}

	}

	public void onMouseEnter()
	{
		
	}

	public void onMouseExit()
	{

	}
}
