using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EZCameraShake;

public class TreantEnemyScript : MonoBehaviour
{
    protected int health = 5;
    GameObject player;
    Transform player_transform;
    public Transform target_bomb;
    private Transform prefab;
    
    public float radius;   
    public float mass = 1.0f;
    public int speed;

   
    private int score_to_give = 10;

    public GameObject hit_effect;

    

    Vector2 line_ended = new Vector2(3f, 0);

    public float max_velocity = 5;
    public float max_clamp = 2;
    public float distance;
    GameObject manager;
    GameManager ref_;
    Scout_Move player_script;
    WanderingBehaviour wandering;
    private RaycastHit2D raycast;
    private float origin_offset = .5f;

    float jump_height = 3f;
    bool hit_enemy = false;
    Vector3 desired, velocity, steering;
    Vector3 x_component_target_player, x_component_self, x_component_target_bomb;

    public SpriteRenderer sprite;

    private bool obstacleCheck = false;
    Vector2 boc_collider = new Vector2(1, 1);
    int rand_speed;
    void Start()
    {
        //Rigidbody2D rigidbody = gameObject.AddComponent<Rigidbody2D>();
        wandering = this.GetComponent<WanderingBehaviour>();
        player = GameObject.Find("Scout");
        player_script = player.GetComponent<Scout_Move>();
        player_transform = player.GetComponent<Transform>();
        rand_speed = Random.Range(4, 7);
        ref_ = GameObject.Find("game_manager").GetComponent<GameManager>();     
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {


        if (collision.gameObject.tag == "Bullet")
        {
            OnEnemyHit();
        }

        if (collision.gameObject.tag == "ground")
        {
            //float y = collision.gameObject.GetComponent<BoxCollider2D>().size.y / 2;


            velocity.y = 0;
        }

    }

    private void OnEnemyHit()
    {
        this.GetComponent<SpriteRenderer>().color = Color.red;

        AddDamage(1);

        ref_.AddScore(score_to_give);

        if (distance > 0 && !ref_.isFleeCollected)
        {
            this.transform.Translate(new Vector3(-2, 0, 0) * Time.deltaTime);
        }

        else if (distance < 0 && !ref_.isFleeCollected)
        {
            this.transform.Translate(new Vector3(2, 0, 0) * Time.deltaTime);
        }

        else if (distance > 0 && ref_.isFleeCollected)
        {
            this.transform.Translate(new Vector3(2, 0, 0) * Time.deltaTime);

        }
        else if (distance < 0 && ref_.isFleeCollected)
        {
            this.transform.Translate(new Vector3(-2, 0, 0) * Time.deltaTime);

        }

        GameObject hit = Instantiate(hit_effect, this.transform.position, Quaternion.identity) as GameObject;
    }
    


    void FixedUpdate()
    {

        RaycastHit2D enemy_info;

        if (!GameObject.Find("Scout") && !GameObject.Find("Scout(Clone)"))
        {
            Debug.Log("no player");

            player = GameObject.FindGameObjectWithTag("AttackingTeam");

        }

        if (!sprite.flipX)
        {
            Debug.DrawRay(new Vector2(this.transform.position.x + 0.2f, this.transform.position.y), Vector3.right, Color.green);
            enemy_info = Physics2D.Raycast(new Vector2(this.transform.position.x + 0.1f, this.transform.position.y), Vector2.right, 0.03f);
        }
        else
        {
            Debug.DrawRay(new Vector2(this.transform.position.x - 0.2f, this.transform.position.y), Vector3.left, Color.red);
            enemy_info = Physics2D.Raycast(new Vector2(this.transform.position.x - 0.1f, this.transform.position.y), Vector2.left, 0.03f);
        }

        if (enemy_info)
        {
            if (enemy_info.transform.tag == "obstacle")  
            {
                //velocity of jump in accordance to size of obstacle
                float obstacle_size = enemy_info.transform.GetComponent<SpriteRenderer>().size.y;
                Debug.Log(obstacle_size);

                if (!obstacleCheck) {
                    obstacleCheck = true;
                    this.GetComponent<Rigidbody2D>().velocity = new Vector2(this.GetComponent<Rigidbody2D>().velocity.x, obstacle_size + 1.5f);
                }
                Debug.Log("jump");
                obstacleCheck = false;
            }

        }    

        this.GetComponent<SpriteRenderer>().color = Color.white;

        HealthCheck();
        //x value of treant position
        x_component_self = new Vector3(this.transform.position.x, 0, 0);

        

        this.transform.Translate(velocity * speed * rand_speed * Time.deltaTime);

        //seeking player behaviour
        if (!GameObject.Find("bomb(Clone)"))
        {
            //limit it to the x-axis
            

            x_component_target_player = new Vector3(player_transform.transform.position.x, 0, 0);

            if (ref_.isFleeCollected) 
            {

                desired = Vector3.Normalize(x_component_self - x_component_target_player) * max_velocity;
                this.GetComponent<SpriteRenderer>().color = new Color(0, 92, 255);
                Debug.Log("fleeing");

                Movement();
                LookAt(2);
            }

            else
            {
                Debug.Log("seeking");
                desired = Vector3.Normalize(x_component_target_player - x_component_self) * max_velocity;
                Movement();

                LookAt(0);
            }
        }


        //arriving at bomb behaviour
        else
            
        {
            //find the instance of the prefab in the scene
            prefab = GameObject.Find("bomb(Clone)").transform;

            x_component_target_bomb = new Vector3(prefab.position.x, 0, 0);

            desired = Vector3.Normalize(x_component_target_bomb - x_component_self) * max_velocity;

            Movement();

            LookAt(1);
        }

    }
    
    private void HealthCheck()
    {
        if (health <= 0)
        {
            health = 0;

            Destroy(this.gameObject);
            

        }    
    }

    private void Movement()
    {
        steering = desired - velocity;

        steering = Vector3.ClampMagnitude(steering, max_clamp * Time.deltaTime);
        steering = steering * (1 / mass);

        velocity = velocity + steering;
        velocity = Vector3.ClampMagnitude(velocity, max_clamp * Time.deltaTime);
    }

    public void AddDamage(int d)
    {
        health -= d;

    }

    private void LookAt(int flag)
    {
        if (flag == 0)
        {

            distance = player_transform.transform.position.x - this.transform.position.x;
            

            if (distance < 0)
            {
                sprite.flipX = true;
            }
            else
            {
                sprite.flipX = false;
            }
        }

        else if (flag == 1)
        {
            //based on instance
            distance = prefab.position.x - this.transform.position.x;
            

            if (distance < 0)
            {
                sprite.flipX = true;
            }
            else
            {
                sprite.flipX = false;
            }
        }

        else if (flag == 2)
        {
             distance = this.transform.position.x - player_transform.transform.position.x;
            

            if (distance > 0)
            {
                sprite.flipX = false;
            }
            else
            {
                sprite.flipX = true;
            }

        }

    }
}

