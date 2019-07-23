using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    public enum State
    {
        spawning,
        waiting,
        counting
    };


    [System.Serializable]
    public class Wave
    {
        public string name;
        public Transform enemy;
        public int count;
        public float rate;
    }

    public Wave[] waves;
    private int next_wave = 1;
    public float time_between_waves = 5f;
    public float wave_countdown;
    private float search_countdown = 1f;
    
    public Transform[] spawn_points;

    protected State state = State.counting;

    void Start()
    {
        


        wave_countdown = time_between_waves;
        

    }

    private void Update()
    {
    

        if (state == State.waiting)
        {

            if (!IsEnemyAlive())
            {
                WaveCompleted();
            }

            else
            {
                return;
            }
        }


        if (wave_countdown <= 0)
        {
            if (state != State.spawning)
            {

                StartCoroutine(Spawn(waves[next_wave]));

            }
        }
        else
        {
            wave_countdown -= Time.deltaTime;
  
        }
    }

    void WaveCompleted()
    {

        state = State.counting;

        wave_countdown = time_between_waves;

        if (next_wave + 1 > waves.Length - 1) 
        {

            next_wave = 0;
            //stat multipliers here

        }

        next_wave++;

    }

    private bool IsEnemyAlive()
    {
        search_countdown -= Time.deltaTime;

        if (search_countdown <= 0f)
        {
            search_countdown = 1f;

            if (GameObject.FindGameObjectsWithTag("enemy").Length <= 0)
            {
                return false;
            }
        }

        return true;
    }

    IEnumerator Spawn(Wave wave_)
    {
        state = State.spawning;

        for (int i = 0; i < wave_.count; i++)
        {

            SpawnEnemy(wave_.enemy);
            yield return new WaitForSeconds(1f / wave_.rate);


        }

        state = State.waiting;


        yield break;
    }


    private void SpawnEnemy(Transform enemy_)
    {
       
        Transform sp = spawn_points[Random.Range(0, spawn_points.Length)];
        Instantiate(enemy_, sp.position, sp.rotation);

    }
}
