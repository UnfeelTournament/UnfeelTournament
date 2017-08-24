using UnityEngine;
using System.Collections;

public class BulletDamage : MonoBehaviour {
    public GameObject weaponOrigin;
    public int playerOrigin;
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public int getDamage()
    {
        int damage = weaponOrigin.GetComponent<ItemEquipped>() == null ? weaponOrigin.GetComponent<ItemMeele>().damage : weaponOrigin.GetComponent<ItemEquipped>().damage;
        GlobalManager._instance._players[playerOrigin].GetComponent<Character>()._damageDealt += damage;
        return damage;
    }
}