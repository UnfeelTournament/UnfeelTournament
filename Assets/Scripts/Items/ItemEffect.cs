using UnityEngine;
using System.Collections;

//this class is for pickupable items in their idle state 
public class ItemEffect : MonoBehaviour
{

    /*Item next Prefab*/
    public GameObject itemPrefab;

    public float scaleRate = 1.5f;
    public float timeDuration = 5f;
    public int heal = 20;
    public float moveSpeedScale = 2f;
    public float jumpForceScale = 2f;
    [HideInInspector]
    public float minScale = 0.1f;
    [HideInInspector]
    public float maxScale = 3.0f; //how big the game object can be.
    [HideInInspector]
    public Vector3 pointB;

    IEnumerator Start()
    {
        var pointA = transform.position;
        pointB = new Vector3((transform.position.x) + (0.1f), transform.position.y, transform.position.z);
        while (true)
        {
            yield return StartCoroutine(MoveObject(transform, pointA, pointB, 3.0f));
            yield return StartCoroutine(MoveObject(transform, pointB, pointA, 3.0f));
        }
    }

    //move back and forth, so the item is easier to spot
    IEnumerator MoveObject(Transform thisTransform, Vector3 startPos, Vector3 endPos, float time)
    {
        var i = 0.0f;
        var rate = 2.0f / time;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            thisTransform.position = Vector3.Lerp(startPos, endPos, i);
            yield return null;
        }
    }

    /*//OLD CODE for item effecting players

    //if get contact with another game object(player)
    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            /*usable Items*/

            /* CAKE */
            //transform it:

            //if (gameObject.transform.name == "cake(Clone)" || gameObject.transform.name == "star_idle(Clone)" || gameObject.transform.name == "cake")
            //{
                //bandage code:
                //decide the sign for scaleRate:
                //if (other.gameObject.transform.localScale.x < minScale)
                //    {
                //        scaleRate = Mathf.Abs(scaleRate);
                //    }
                //    else if (other.gameObject.transform.localScale.x > maxScale)
                //    {
                //        scaleRate = -Mathf.Abs(scaleRate);
                //    }

                //other.gameObject.transform.localScale += (new Vector3(1,1,1) * scaleRate);
                //this.gameObject.SetActive(false); //make it invisible
                //Destroy(this.gameObject); //delete the item game object.
            //}

            /*PICKAble items*/

            //alreadyEquipped(other.transform);
            //other.transform.GetChild(0) = hand, we need hand!
            /*
            if (gameObject.transform.name == "baseballBat_idle(Clone)" || gameObject.transform.name == "baseballBat_idle")
            {
                GameObject melee = (GameObject)Instantiate(itemPrefab, other.transform.position, other.transform.rotation);
                if (melee.transform.GetChild(0).name == "hilt")
                {
                    melee.transform.localPosition = new Vector3(melee.transform.position.x, melee.transform.position.y + 0.5f, melee.transform.position.z);
                }
                melee.transform.parent = other.transform;
            }

            if (gameObject.transform.name == "gun_Idle(Clone)" || gameObject.transform.name == "gun_Idle")
            {
                GameObject gun = (GameObject)Instantiate(itemPrefab, other.transform.position, other.transform.rotation);
                gun.transform.parent = other.transform;
            }

            
            
            //Destroy(this.gameObject);
        }

    }
    

    // destroys the previously equipped weapon
    void alreadyEquipped(Transform shooter)
    {
        if (shooter.childCount > 1)
        {
            Destroy(shooter.GetChild(1).gameObject);
        }
    }
    
     */
}