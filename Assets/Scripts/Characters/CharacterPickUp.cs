using UnityEngine;
using System.Collections;

//If the player collides with a pickup layer object. (If multiple objects, pick random)
public class CharacterPickUp : MonoBehaviour {
    //Pick Up Values
    public float _pickUpRange;

    //Equip Key (Swap equipped weapon with collided weapon)

    //Pick Up Audio

    //Pick Up References
    private Animator _anim;             
    private CircleCollider2D _collider; 
                                        
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
