using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegularAmmoScript : MonoBehaviour
{
    [SerializeField]
    private int damage_inflict_ammo = 1;

    TreantEnemyScript treant_instance;
    public TreantEnemyScript enemy;
    


    // Start is called before the first frame update
    void Start()
    {
        treant_instance = enemy.GetComponent<TreantEnemyScript>();
       
    }

 

    // Update is called once per frame
    void Update()
    {
       
    }



    public int GetDamage()
    {
        return damage_inflict_ammo;
    }
}
