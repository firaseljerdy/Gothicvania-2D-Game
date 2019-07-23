using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool isCoinCollected = false;
    public bool isFleeCollected = false;

    public AudioClip theme_music;
    public AudioSource music_source;
   
    GameObject player_ref;
    private Scout_Move player;
    public bool isDead = false;
    public GameObject image_bomb;
    public GameObject image_flee;
    public Image box_inventory;
    public GameObject audio_object;

    public GameObject regular_ammo_instance;

    public GameObject treant_enemy_instance;
    RegularAmmoScript regularAmmo;
    TreantEnemyScript treant_enemy;
    private int health_treant;

    FrenzyScript frenzy_sky, frenzy_sun;
    public GameObject frenzy_sky_obj, frenzy_sun_obj;

    Color original_color_sky;
    Color original_color_sun;
    public ParticleSystem particle_frenzy;
    GameObject main_camera;
    int score = 0;
    public GameObject score_txt;


    private void Awake()
    {
        player_ref = GameObject.Find("Scout");

        //there was trouble loading the camera shaker
        main_camera = GameObject.FindGameObjectWithTag("MainCamera");
        main_camera.AddComponent<EZCameraShake.CameraShaker>();

    }




    // Start is called before the first frame update
    void Start()
    {
        music_source.clip = theme_music;


        score = 0;

        score_txt.GetComponent<Text>().text = "";

        player = player_ref.GetComponent<Scout_Move>();

        image_bomb.SetActive(false);
        image_flee.SetActive(false);

        frenzy_sky = frenzy_sky_obj.GetComponent<FrenzyScript>();
        frenzy_sun = frenzy_sun_obj.GetComponent<FrenzyScript>();

        frenzy_sun.enabled = false;
        frenzy_sky.enabled = false;
 
        original_color_sky = frenzy_sky_obj.GetComponent<Light>().color;
        original_color_sun = frenzy_sun_obj.GetComponent<Light>().color;
    }

    // Update is called once per frame
    void Update()
    {

        if (!music_source.isPlaying)
        {
            music_source.Play();

        }

        

        score_txt.GetComponent<Text>().text = score + "";

        ManageInventoryImages();

        if (isFleeCollected) {

            music_source.pitch = 0.63f;


            frenzy_sun.enabled = true;
            frenzy_sky.enabled = true;
            if (!particle_frenzy.isPlaying)
            {
                particle_frenzy.Play();
            }
        }
        else
        {

            music_source.pitch = 1f;

            frenzy_sun.enabled = false;
            frenzy_sky.enabled = false;

            frenzy_sky_obj.GetComponent<Light>().color = original_color_sky;
            frenzy_sun_obj.GetComponent<Light>().color = original_color_sun;
            if (particle_frenzy.isPlaying)
            {
                particle_frenzy.Stop();
            }
        }


    }

    public void AddScore(int give)
    {

        score += give;
        


    }

    private void ManageInventoryImages()
    {
        if (isCoinCollected)
        {
            image_bomb.SetActive(true);
            image_flee.SetActive(false);
        }

        else
        {
            image_bomb.SetActive(false);
        }

        if (player.haveFleeCoin)
        {
            image_flee.SetActive(true);
            image_bomb.SetActive(false);
        }

        else
        {
            image_flee.SetActive(false);
        }
    }
}
