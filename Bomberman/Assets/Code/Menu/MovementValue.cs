using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovementValue : MonoBehaviour
{
    public GameObject player;
    // Update is called once per frame
    void Update()
    {
        if(this.gameObject.GetComponent<Slider>().value==0)
        {
            player.GetComponent<Movement_physics>().Movement = true;
        }
        else
        {
            player.GetComponent<Movement_physics>().Movement = false;
        }
    }
}
