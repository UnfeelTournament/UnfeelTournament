using UnityEngine;
using System.Collections;

public class SoundController : MonoBehaviour {
    public AudioSource sound;

    public AudioClip select;
    public AudioClip back;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void setAudioSelect()
    {
        if (sound.isPlaying) sound.Stop();
        sound.clip = select;
        sound.Play();
    }

    public void setAudioBack()
    {
        if (sound.isPlaying) sound.Stop();
        sound.clip = back;
        sound.Play();
    }
}
