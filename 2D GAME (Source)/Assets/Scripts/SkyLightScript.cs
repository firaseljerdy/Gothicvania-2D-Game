using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyLightScript : MonoBehaviour
{
   

    // Start is called before the first frame update
    void Start()
    {

      

    }

    private void Update()
    {
        
      
        if (this.GetComponent<Light>().intensity >= 1.10f)
        {
            Debug.Log("bringing light down");
            this.GetComponent<Light>().intensity = Mathf.Lerp(this.GetComponent<Light>().intensity, 1.06f, 1.2f * Time.deltaTime);
        }     

    }

  

    
  

}
