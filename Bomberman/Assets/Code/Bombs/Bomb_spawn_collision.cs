using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Bomb_spawn_collision : MonoBehaviour {
    public GameObject creator { get; set; }
    public GameObject explosion_vertical { get; set; }
    public GameObject explosion_horizontal { get; set; }
    public AudioClip explosion { get; set; }
    public AudioMixerGroup mixer { get; set; }
    public bool collided { get; set; }
    private int bounce_count;
    private bool kicked = false;
    private void Start()
    {
        bounce_count = 0;
        collided = false;
        StartCoroutine(CheckForCollision());
    }
    private void OnTriggerEnter(Collider other)
    {
        if((other.tag=="Boxes")||(other.tag=="Walls")||(other.tag=="Bombs")||(other.tag=="Game_End_Box"))
        {
            collided = true;
        }

        if ((other.tag == "Map Objects") && (bounce_count < creator.GetComponent<Additional_power_ups>().bounce_limit))
        {
            bounce_count++;
        }
        else if (other.tag == "Map Objects")
        {
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if ((other.tag == "Map Objects") && (bounce_count < creator.GetComponent<Additional_power_ups>().bounce_limit))
        {
            bounce_count++;
        }
        else if ((other.tag == "Map Objects")&&(!kicked))
        {
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if ((collision.gameObject.tag == "Player") || (collision.gameObject.tag == "AI"))
        {
          if (collision.gameObject.GetComponent<Additional_power_ups>().bomb_kick)
          {
                Vector3 direction = new Vector3(0f,0f,0f);
                gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
                if (collision.gameObject.tag == "Player")
                {
                    direction = collision.gameObject.GetComponent<Transform>().forward; // finding the way player is watching
                }
                else
                {
                    switch (collision.gameObject.GetComponent<AI_Movement>().moving_direction)
                    {
                        case "Front":
                            direction = new Vector3(0f, 0f, 1f);
                            break;
                        case "Back":
                            direction = new Vector3(0f, 0f, -1f);
                            break;
                        case "Left":
                            direction = new Vector3(-1f, 0f, 0f);
                            break;
                        case "Right":
                            direction = new Vector3(1f, 0f, 0f);
                            break;
                        case "":
                            direction = new Vector3(0f, 0f, 0f);
                            break;
                    }
                }
                gameObject.GetComponent<Rigidbody>().AddForce(direction * 10f, ForceMode.Impulse);
                gameObject.GetComponent<SphereCollider>().material = null;
                kicked = true;
            }
        }
        else if ((collision.gameObject.tag == "Map Objects") && (bounce_count < creator.GetComponent<Additional_power_ups>().bounce_limit))
        {
            bounce_count++;
        }
        else if ((collision.gameObject.tag == "Map Objects") && (!kicked))
        {
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY| RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        }
    }
    private IEnumerator CheckForCollision()
    {
        yield return null;
        this.gameObject.GetComponent<Transform>().position = this.gameObject.GetComponent<Transform>().position + Vector3.zero;
        yield return null;
        if (collided)
        {
            Destroy(this.gameObject);
            if (creator.tag == "Player")
            {
                creator.GetComponent<Shooting_physics>().allowed_to_throw = true;
                if (creator.GetComponent<Shooting_physics>().count > 0)
                {
                    creator.GetComponent<Shooting_physics>().count--;
                }
            }
            else
            {
                creator.GetComponent<AI_Shooting>().allowed_to_throw = true;
                if (creator.GetComponent<AI_Shooting>().count > 0)
                {
                    creator.GetComponent<AI_Shooting>().count--;
                }
            }
        }
        else
        {
            this.gameObject.AddComponent<Bomb_explosion>();
            this.gameObject.GetComponent<Bomb_explosion>().creator = creator;
            this.gameObject.GetComponent<Bomb_explosion>().explosion_vertical = explosion_vertical;
            this.gameObject.GetComponent<Bomb_explosion>().explosion_horizontal = explosion_horizontal;
            this.gameObject.GetComponent<Bomb_explosion>().explosion = explosion;
            this.gameObject.GetComponent<Bomb_explosion>().mixer = mixer;
        }
    }
}
