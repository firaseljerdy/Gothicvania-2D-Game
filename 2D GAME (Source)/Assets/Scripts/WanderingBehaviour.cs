using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingBehaviour : MonoBehaviour
{

    //Circle radius
    public float circleRadius = 3.5f;
    //Circle distance
    public float circleDistance = 5;

    public float randomAngle = 5;
    public float mass = 1.0f;
    public float maxForce = 3.0f;
    public float maxSpeed = 10.0f;
    

    //timer control
    public int cycle = 10;
    int timer = 0;

    Vector3 wanderTarget = new Vector3(0, 0, 0);

    Vector3 steering;
    Vector3 acceleration, velocity, last_wander_pos;

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        steering = Wandering();

        steering = Vector3.ClampMagnitude(steering, maxForce);
        acceleration = steering / mass;
        velocity = Vector3.ClampMagnitude(velocity + acceleration, maxSpeed);

        //Move the object 
        transform.position += velocity * Time.deltaTime;
        
    }

    Vector3 Wandering()
    {
        timer++;
        if (timer >= cycle)
        {
            wanderTarget += (circleDistance * -transform.forward) + (transform.position - last_wander_pos);

            wanderTarget += new Vector3(Random.Range(-randomAngle, randomAngle), 0, transform.position.z);

            last_wander_pos = transform.position;

            //Circle
            Vector3 to_wander_target = Vector3.Normalize(wanderTarget - transform.position) * circleRadius;

            to_wander_target += (transform.forward * circleDistance);

       

            wanderTarget = transform.position + to_wander_target;

       

            Vector3 desired_velocity = Vector3.Normalize(to_wander_target) * maxSpeed;

            Vector3 steering = desired_velocity - velocity;
            //Hack to keep the system on the XZ plane
            steering = new Vector3(steering.x, 0, transform.position.z);

            //Reset the timer
            timer = 0;

            return steering;
        }
        else
        {
            return Vector3.zero;
        }

    }
}

