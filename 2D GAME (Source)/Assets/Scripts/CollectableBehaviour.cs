using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CollectableBehaviour : MonoBehaviour
{

    public float speed;
    private float waitTime;
    public float startWaitTime;
    public Transform moveToPosition;
    public float minX, maxX, minY, maxY;
    Transform player;
    


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Scout").transform;
        waitTime = startWaitTime;
        //random position of empty obj
        moveToPosition.position = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY));

     
    }

    // Update is called once per frame
    void Update()
    {   //move towards empty obj
        transform.position = Vector2.MoveTowards(transform.position, moveToPosition.position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, moveToPosition.position) < 0.2f)
        {
            if (waitTime <= 0)
            {
                //when waiting time is over reposition
                moveToPosition.position = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
                waitTime = startWaitTime;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }


        }

    }

}

