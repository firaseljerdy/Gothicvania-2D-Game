using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowScript : MonoBehaviour
{
    GameObject player_ref;
    GameObject map_boundary_left;
    GameObject map_boundary_right;

    Vector2 player_pos;
    Vector2 map_boundary_left_pos;
    Vector2 map_boundary_right_pos;
    public Vector2 new_camera_position;

    public float x_axis_left = -0.427f;
    public float x_axis_right = 2.468735f;

    private Vector3 camera_offset_z = new Vector3(0, 0, -10);
    private Vector3 camera_offset_y = new Vector3(0, 0.7f, 0);
    GameObject dead_limit;

    // Start is called before the first frame update
    void Start()
    {
        player_ref = GameObject.Find("Scout");
        
        SetMapBoundaries();

    }

    private void SetMapBoundaries()
    {
        //get the location of the boundaries
        map_boundary_left = GameObject.Find("border_left");

        map_boundary_left_pos = map_boundary_left.transform.position;


        map_boundary_right = GameObject.Find("border_right");

        map_boundary_right_pos = map_boundary_right.transform.position;
    }

    

    // Update is called once per frame
    void Update()
    {
        if (player_ref)
        {
            player_pos = player_ref.transform.position;
            UpdateCameraPosition();
        }

        else
        {

            player_pos = new Vector2(2f, 2f);

        }

        if (GameObject.Find("Scout"))
        {
            if (player_ref.GetComponent<Scout_Move>().collision_with_enemy)
            {
                this.transform.position = player_ref.GetComponent<Scout_Move>().last_player_position_before_death + camera_offset_z + camera_offset_y;
            }
        }
   

        //checks for left or right passing
        if (this.transform.position.x <= map_boundary_left_pos.x)
        {
            StopUpdateCameraPosition(0);
        }

        if(this.transform.position.x >= map_boundary_right_pos.x)
        {
            StopUpdateCameraPosition(1);
        }    
    }

    void UpdateCameraPosition()
    {
        this.transform.position = player_ref.transform.position + camera_offset_z + camera_offset_y;

        new_camera_position = this.transform.position;     
    }

    void StopUpdateCameraPosition(int flag)
    {

        switch (flag)
        {
            case 0:

                Vector3 hold_left = new Vector3(x_axis_left, transform.position.y, transform.position.z);

                this.transform.position = hold_left;

                break;

            case 1:

                Vector3 hold_right = new Vector3(x_axis_right, transform.position.y, transform.position.z);

                this.transform.position = hold_right;

                break;
             
        }
    }
}
