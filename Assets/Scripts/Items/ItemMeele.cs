using UnityEngine;
using System.Collections;
//this class is for melee item
public class ItemMeele : MonoBehaviour {

    public GameObject AreaOfEffect;
    public float attackSpeed = 1;
    public int damage = Constants.BULLET_DAMAGE;
    private float timeToSwing = 0;

    public string attackButton ="Attack1"; //from player button
    private Transform swingArea;

    public float knockback;

    public int playerOrigin;

    void Start()
    {
        swingArea = transform.FindChild("SwingArea");
       //attackButton = GetComponentInParent<Character>()._attackAxis;  //for testing  
    }

    // Update is called once per frame
    void Update()
    {
        timeToSwing -= Time.deltaTime;
    }

    public void swing(bool faceRight)
    {
        if (timeToSwing > 0) return;

        timeToSwing = attackSpeed;

        int direction = faceRight ? -1 : 1;
        int y = (int)swingArea.rotation.y == 0 ? direction : (int)swingArea.rotation.y;

        Quaternion temp = new Quaternion(0, y, 0, 0);
        //Debug.Log("SWING!");

        gameObject.GetComponent<Animator>().SetTrigger("Swing");
        GameObject swing = (GameObject)Instantiate(AreaOfEffect, swingArea.position, temp);

        swing.GetComponent<BulletDamage>().weaponOrigin = this.gameObject;
        swing.GetComponent<BulletDamage>().playerOrigin = playerOrigin;
    }
}
