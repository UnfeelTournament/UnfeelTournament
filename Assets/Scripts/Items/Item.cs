using UnityEngine;
using System.Collections;

//this class is for spawner(item)

public class Item : MonoBehaviour {

    //Public can be accessed thru Unity Inspector:
    public float timer = 5;
    public GameObject[] Possibleitems;

    /* Time to respawn and random items*/
    private float saveTimer;
    private int randNum;

    void Start()
    {
        createItemObject();
        saveTimer = timer;
    }

    // Update is called once per frame
    //checking with the time, if there is no object, spawn a new one
    void Update()
    {
        if (transform.childCount == 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = saveTimer;
                createItemObject();
            }
        }
    }

    //set in the Inspector, chance of each item to respawn
    void createItemObject()
    {
        randNum = Random.Range(0, Possibleitems.Length);
        GameObject spawn = (GameObject)Instantiate(Possibleitems[randNum], transform.position, transform.rotation);
        spawn.transform.parent = transform;
    }
}
