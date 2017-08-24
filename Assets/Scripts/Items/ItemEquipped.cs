using UnityEngine;
using System.Collections;
//this class is for weapon, mostly kind of guns
public class ItemEquipped : MonoBehaviour
{

    /* bullet speed */
    public float fireRate = 0;

    public int damage = Constants.BULLET_DAMAGE;
    /* bullet Prefab variables*/
    public GameObject bulletPrefab;
    public Transform firePoint;

    /* bullet control */
    public string shootButton = "TestAttack";
    private float timeToFire = 0;
    // public static int timesCalled = 0;
    public float knockback;

    public int playerOrigin;
 
    void Start()
    {
        firePoint = transform.FindChild("firepoint");
        //Debug.Log("Original Fire Point Rotation: " + firePoint.rotation);
        //shootButton = GetComponentInParent<Character>()._attackAxis;
    }

    // Update is called once per frame
    void Update()
    {
        timeToFire -= Time.deltaTime;
        //need default weapon?
        //if (Input.GetButton(shootButton) && gameObject.transform.name == "Gun_Ready(Clone)" && Time.time > timeToFire)
        //{
        //    //GetComponent<AudioSource>().Play();
        //    timeToFire = Time.time + 1 / fireRate;
        //    Shoot();
        //}
        //else if (Input.GetButtonDown(shootButton) && Time.time > timeToFire)
        //{
        /* Debug.Log("Rotation: " + firePoint.rotation);
         if (Input.GetButtonDown(shootButton) && Time.time > timeToFire)
         { 
             //GetComponent<AudioSource>().Play();
             timeToFire = Time.time + 1 / fireRate;
             Shoot();
         }*/
        //}
    }

    public void Shoot(bool faceRight)
    {
        //Debug.Log("TIME TO FIRE: " + timeToFire);
        //timesCalled++;
        if (timeToFire > 0) return;
        //Debug.Log("Times called before firing: " + timesCalled);
        //timesCalled = 0;
        timeToFire = fireRate;

        int direction = faceRight ? -1 : 1;
        int y = (int)firePoint.rotation.y == 0 ? direction : (int)firePoint.rotation.y;
        //Debug.Log("Position: " + firePoint.position);
        //Debug.Log("FACING RIGHT: " + faceRight);
        //Debug.Log("DIRECTION: " + direction);
        //Debug.Log("ROTATION.Y: " + y);

        Quaternion temp = new Quaternion(0, y, 0, 0);       //Debug.Log("DIRECTION * ROTATION: " + temp.y);

        /* Shotgun: */
        if (transform.name == "Bubble_Shotgun_Ready(Clone)" || transform.name == "Bubble_Shotgun_Ready")
        {
            GameObject bullet1 = (GameObject)Instantiate(bulletPrefab, firePoint.position, temp);
            GameObject bullet2 = (GameObject)Instantiate(bulletPrefab, firePoint.position, temp);
            bullet2.transform.Rotate(Vector3.forward * -6);
            GameObject bullet3 = (GameObject)Instantiate(bulletPrefab, firePoint.position, temp);
            bullet3.transform.Rotate(Vector3.forward * 6);
            GameObject bullet4 = (GameObject)Instantiate(bulletPrefab, firePoint.position, temp);
            bullet4.transform.Rotate(Vector3.forward * -14);
            GameObject bullet5 = (GameObject)Instantiate(bulletPrefab, firePoint.position, temp);
            bullet5.transform.Rotate(Vector3.forward * 14);

            bullet1.GetComponent<BulletDamage>().weaponOrigin = this.gameObject;
            bullet1.GetComponent<BulletDamage>().playerOrigin = playerOrigin;
            bullet2.GetComponent<BulletDamage>().weaponOrigin = this.gameObject;
            bullet2.GetComponent<BulletDamage>().playerOrigin = playerOrigin;
            bullet3.GetComponent<BulletDamage>().weaponOrigin = this.gameObject;
            bullet3.GetComponent<BulletDamage>().playerOrigin = playerOrigin;
            bullet4.GetComponent<BulletDamage>().weaponOrigin = this.gameObject;
            bullet4.GetComponent<BulletDamage>().playerOrigin = playerOrigin;
            bullet5.GetComponent<BulletDamage>().weaponOrigin = this.gameObject;
            bullet5.GetComponent<BulletDamage>().playerOrigin = playerOrigin;
        }
        else if (transform.name == "Launcher_Ready(Clone)" || transform.name == "Launcher_Ready")
        {
            GameObject bullet = (GameObject)Instantiate(bulletPrefab, firePoint.position, temp);
            if(direction == -1)
                bullet.transform.Rotate(Vector3.forward * -35);
            else
                bullet.transform.Rotate(Vector3.forward * 35);
            bullet.GetComponent<BulletDamage>().weaponOrigin = this.gameObject;
            bullet.GetComponent<BulletDamage>().playerOrigin = playerOrigin;
        }
        else
        {
            GameObject bullet = (GameObject)Instantiate(bulletPrefab, firePoint.position, temp);
            bullet.GetComponent<BulletDamage>().weaponOrigin = this.gameObject;
            bullet.GetComponent<BulletDamage>().playerOrigin = playerOrigin;
        }
    }
}