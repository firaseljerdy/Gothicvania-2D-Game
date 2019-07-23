using UnityEngine;
using System.Collections;
using EZCameraShake;

public class Scout_Move : MonoBehaviour {

    public Transform sprite;
    // Use this for initialization
    Animator anim;
    
    public bool scout_spawn;
    public static float Speed = 1.33f;
    public static float Jump = 3f;
    
    public static bool grounded;
    public bool ground;
    public static bool Scout = true;

    public Rigidbody2D rigid;
    public GameObject enemy;
    public GameObject bullet;
    public GameObject collectable_bomb;
    public GameObject collectable_flee;
    GameObject manager;
    public GameObject pickup;
    public bool isOverObstacle;
    GameObject dead_limit;
    public AudioClip shooting_sound;
    
    public AudioSource source;

    public bool haveFleeCoin = false;
    public Vector3 last_player_position_before_death;
    Movement Move = new Movement();

    RaycastHit2D hit;
    GameManager ref_;

    float fireRate = 0.08f;
    float nextFire = 0.0f;

    public GameObject prefab_bomb;
    public GameObject gunPointOne;
    public bool collision_with_enemy = false;


    void Start() {
        manager = GameObject.Find("game_manager");
        collectable_bomb = GameObject.Find("collectable_bomb");
        collectable_flee = GameObject.Find("collectable_flee");
        anim = GetComponent<Animator>();
        dead_limit = GameObject.Find("dead_limit");
        source.clip = shooting_sound;
    }
    void FixedUpdate() {

        GroundDetection();

        ground = grounded;

        ref_ = manager.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("Speed", Mathf.Abs(rigid.velocity.x));
        anim.SetBool("touchingGround", grounded);
        Move.Motion(Speed, Jump, rigid, grounded, Scout, sprite);
        CheckUserInputForShooting();

        if (collision_with_enemy)
        {
            collision_with_enemy = false;         
        }
        
    }

    private void CheckUserInputForShooting()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
           
            source.Play();
            
            nextFire = Time.time + fireRate;
            CameraShaker.Instance.ShakeOnce(0.4f, 0.4f, 0.1f, 0.1f);
            Shoot();
        }
        //bomb
        if (Input.GetButtonDown("Fire2") && ref_.isCoinCollected)
        {
            //Debug.Log("spawn");
            GameObject obj = Instantiate(prefab_bomb, this.transform.position, Quaternion.identity) as GameObject;
            ref_.isCoinCollected = false;
        }

        if (Input.GetButtonDown("Fire2") && haveFleeCoin)
        {
            haveFleeCoin = false;
            ref_.isFleeCollected = true;
            

            GameObject flee_ps = Instantiate(pickup, new Vector2(2,2), Quaternion.identity) as GameObject;
            //flee lasts for 10 seconds
            Invoke("SetBoolBack", 10);
        }
    }

    private void SetBoolBack()
    {       
        ref_.isFleeCollected = false;
        //Debug.Log("set bool back");
        Destroy(collectable_flee);
    }

    public void GroundDetection() {
        hit = Physics2D.Raycast(GameObject.Find("Scout_Feet").transform.position, Vector2.down);
        if (hit.distance < 0.03f) {
            grounded = true;
        }
        if (hit.distance > 0.03f) {
            grounded = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "collectable_bomb") 
        {

            Destroy(collectable_bomb);
            ref_.isCoinCollected = true;        
        }

        //when there is the cloak and pick up bomb
        if (collision.name == "collectable_bomb" && haveFleeCoin)
        {
            

            haveFleeCoin = false;       

        }

        if (collision.name == "collectable_flee") 
        {
            collectable_flee.GetComponent<SpriteRenderer>().enabled = false;
            collectable_flee.GetComponent<CircleCollider2D>().enabled = false;

            haveFleeCoin = true;

            
        }
        //when there is the bomb and pick up cloak
        if (collision.name == "collectable_flee" && ref_.isCoinCollected)
        {
            Debug.Log("drop bomb for flee");

            collectable_flee.GetComponent<SpriteRenderer>().enabled = false;
            collectable_flee.GetComponent<CircleCollider2D>().enabled = false;

            ref_.isCoinCollected = false;

        }

        if (collision.gameObject.tag == "dead_limit")
        {
            Debug.Log("dead");
            manager.GetComponent<GameManager>().isDead = true;
            //Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "enemy_head") 
        {
            rigid.velocity = new Vector2(rigid.velocity.x, 3.5f);         
        }

        if (collision.gameObject.tag == "enemy" && !ref_.isFleeCollected)
        {
            collision_with_enemy = true;
            
            last_player_position_before_death = new Vector3(transform.position.x, transform.position.y, -10);
            StartCoroutine(Rotate(3));
                     
            rigid.constraints = RigidbodyConstraints2D.None;

            StartCoroutine(PauseEffect(1));
            if (!Movement.facingRight)
            {             
                rigid.velocity = new Vector2(rigid.velocity.x + 2f, 3.5f);
            }
            else
            {        
                 rigid.velocity = new Vector2(rigid.velocity.x - 2f, 3.5f);
            }

            
        }

   
    }



    private IEnumerator Rotate(float duration)
    {
        float time_remaining = duration;
        while (time_remaining > 0)
        {
            time_remaining -= Time.deltaTime;

            Vector3 currentAngle = new Vector3(
                Mathf.LerpAngle(transform.eulerAngles.x, 0, Time.deltaTime),
                Mathf.LerpAngle(transform.eulerAngles.y, 0, Time.deltaTime),
                Mathf.LerpAngle(transform.eulerAngles.z, 181, Time.deltaTime));

            transform.eulerAngles = currentAngle;
            yield return null;
        }
    }
    //pause for a number of p seconds
    private IEnumerator PauseEffect(int p)
    {
        //temp settings
        Time.timeScale = 0.0f;

        this.GetComponent<Scout_Move>().enabled = false;

        float pause_end_time = Time.realtimeSinceStartup + 1;

        while (Time.realtimeSinceStartup < pause_end_time)
        {
            yield return 0;
        }
       //initial settings
        this.GetComponent<Scout_Move>().enabled = true;

        Time.timeScale = 1;
      
        this.GetComponent<BoxCollider2D>().enabled = false;
        this.GetComponent<Scout_Move>().enabled = false;
    }

    public void Shoot(){

		GameObject bulletMid = Instantiate (bullet, gunPointOne.transform.position, gunPointOne.transform.rotation) as GameObject;

		bulletMid.tag = "Bullet";

		Destroy (bulletMid, 0.35f);

		if(Movement.facingRight){
			rigid.AddForce (-rigid.transform.right * 15);

			bulletMid.GetComponent<Rigidbody2D> ().AddForce (bulletMid.GetComponent<Rigidbody2D> ().transform.right  * 300);
		}
		if(!Movement.facingRight){
			rigid.AddForce (rigid.transform.right * 15);
			bulletMid.GetComponent<Rigidbody2D> ().AddForce (-bulletMid.GetComponent<Rigidbody2D> ().transform.right  * 300);
		}      
    }
}
