using UnityEngine;
using System.Collections;

public class CharacterAttack : MonoBehaviour {
    //Damage Modifiers - Depends on items
    public float _damage;       //Amount of damage a shot does to a player
    public float _spread;       //The angle of spread when firing (Higher angle means less accuracy)
    public float _range;        //How far the shot travels before it is destroyed (without colliding)
    public float _speed;        //How fast the shot travels
    public float _frequency;    //How frequently you can fire a shot
    //Weapon Equipped

    //Sounds for Attacking
    AudioSource _attackAudio;
    AudioClip _attackAudioClip;

    Animator _anim;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //Attacks with the weapon in a shape in front of the character
    public void attack()
    {

    }

    //Secondary attack (if it is needed)
    public void attackSecondary()
    {

    }
}
