using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class BombScript : MonoBehaviour
{
    private int bomb_damage = 10;
    private float bomb_radius = 1f;
    private float timePass = 0.0f;
    int updateCount = 0;


    private float bomb_time = 3.5f;

    public bool isBombActive = false;

    public GameObject explosion;
    public ParticleSystem fuse;
    
    GameObject directional_light_sky;
    CameraShake camera_shake;
    GameManager manager;
    private Vector2 point_center;
    
   
    // Start is called before the first frame update
    void Start()
    {
        

        camera_shake = GameObject.Find("Main Camera").GetComponent<CameraShake>();
        directional_light_sky = GameObject.Find("Directional Light");

        manager = GameObject.Find("game_manager").GetComponent<GameManager>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Timer();
        
    }

    private void Timer()
    {
        timePass += Time.deltaTime;
        

        if (timePass >= bomb_time - 0.01f)
        {
            timePass = 0.0f;

        
            Debug.Log("implode");


           

            point_center = new Vector2(transform.position.x, transform.position.y);
            Collider2D[] hits = Physics2D.OverlapCircleAll(point_center, bomb_radius);

            foreach (Collider2D hit in hits)
            {
                if (hit.tag == "enemy")
                {
                                        
                    hit.GetComponent<TreantEnemyScript>().AddDamage(bomb_damage);
                    manager.AddScore(10);
                }
            }
    
            Destroy(this.gameObject);        
            updateCount = updateCount + 1;
        }
    }

    void OnDrawGizmos()
    {
        // Display the explosion radius when selected
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, bomb_radius);
    }

    protected void OnDestroy()
    {
        Debug.Log("bomb destroyed");
        
        directional_light_sky.GetComponent<Light>().intensity = 8f;
        GameObject particle_system = Instantiate(explosion, this.transform.position, Quaternion.identity) as GameObject;
        CameraShaker.Instance.ShakeOnce(5f, 5f, 0.09f, 2f);
    }

}
