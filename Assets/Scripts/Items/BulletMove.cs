using UnityEngine;
using System.Collections;

public class BulletMove : MonoBehaviour
{
    public GameObject onImpact;
    public float maxSpeed = 10f;
    public float range = 0.5f;
    public Vector3 origin;
    // Use this for initialization
    void Start()
    {
        //Debug.Log("ORIGIN: " + origin);
        origin = transform.position;
        AudioSource As = GetComponent<AudioSource>();
        if(As)
            GetComponent<AudioSource>().Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (((int)Mathf.Abs(origin.x - transform.position.x) > range))
        {
            if(transform.name == "BazookaBullet(Clone)" || transform.name == "BazookaBullet")
            {
                GameObject explosion = (GameObject)Instantiate(onImpact, transform.position, transform.rotation);
                //explosion.GetComponent<BulletDamage>().weaponOrigin = this.gameObject.GetComponent<BulletDamage>().weaponOrigin;
                //explosion.GetComponent<BulletDamage>().playerOrigin = this.gameObject.GetComponent<BulletDamage>().playerOrigin;
                Destroy(explosion,1f);
            }
            Destroy(this.gameObject);
        }
        Vector3 pos = transform.position;
        //Debug.Log("Bullet Position: " + pos);
        //Debug.Log("Bullet Rotation: " + transform.rotation);

        Vector3 velocity = new Vector3(maxSpeed * Time.deltaTime, 0, 0);
        int direction = transform.rotation.y < 0 ? -1 : 1;
        pos += transform.rotation * (direction * velocity);

        //Debug.Log("Direction: " + direction + " : " + transform.rotation.y);
        //Debug.Log("Bullet Position: " + pos);
        //Debug.Log("Bullet Rotation: " + transform.rotation.y);
        transform.position = pos;
    }

    void FixedUpdate()
    {

    }    
}