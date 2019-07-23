using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class FrenzyScript : MonoBehaviour
{
    float time_left = 1f;
    float saved_time;
    

    // Start is called before the first frame update
    void Start()
    {
        saved_time = time_left;
    }

    // Update is called once per frame
    void Update()
    {
        CameraShaker.Instance.ShakeOnce(0.4f, 0.4f, 0.3f, 0.3f);
        time_left -= Time.deltaTime;
        if (time_left < 0)
        {
            

            this.GetComponent<Light>().color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f, 1f, 1f);
            time_left = saved_time;

            
        }
    }
}
