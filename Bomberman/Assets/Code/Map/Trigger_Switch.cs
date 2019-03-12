using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_Switch : MonoBehaviour {
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Ground")
        {
            this.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            Transform transform = this.gameObject.GetComponent<Transform>();
            transform.localPosition = new Vector3(transform.localPosition.x, 1f, transform.localPosition.z);
            this.gameObject.GetComponentInChildren<BoxCollider>().isTrigger = true;
        }
        else if((collision.gameObject.tag=="Player"|| collision.gameObject.tag == "AI")&&(this.gameObject.transform.localPosition.y>1.5f))
        {
            collision.gameObject.GetComponent<Additional_power_ups>().lifes_count = -1;
        }
        else if((collision.gameObject.tag=="Bombs") && (this.gameObject.transform.localPosition.y > 1.5f))
        {
            Destroy(collision.gameObject);
            if (collision.gameObject.GetComponent<Bomb_spawn_collision>().creator.tag == "Player")
            {
                collision.gameObject.GetComponent<Bomb_spawn_collision>().creator.GetComponent<Shooting_physics>().allowed_to_throw = true;
                if (collision.gameObject.GetComponent<Bomb_spawn_collision>().creator.GetComponent<Shooting_physics>().count > 0)
                {
                    collision.gameObject.GetComponent<Bomb_spawn_collision>().creator.GetComponent<Shooting_physics>().count--;
                }
            }
            else
            {
                collision.gameObject.GetComponent<Bomb_spawn_collision>().creator.GetComponent<AI_Shooting>().allowed_to_throw = true;
                if (collision.gameObject.GetComponent<Bomb_spawn_collision>().creator.GetComponent<AI_Shooting>().count > 0)
                {
                    collision.gameObject.GetComponent<Bomb_spawn_collision>().creator.GetComponent<AI_Shooting>().count--;
                }
            }
        }
    }
}
