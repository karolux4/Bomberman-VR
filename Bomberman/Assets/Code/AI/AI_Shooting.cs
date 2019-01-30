using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AI_Shooting : MonoBehaviour {

    public GameObject bomb;
    public GameObject explosion_vertical;
    public GameObject explosion_horizontal;
    public PhysicMaterial bounce;
    public int count { get; set; }
    public bool allowed_to_throw { get; set; }
    public float strength;
    public float bomb_collision_radius;
    public AudioClip explosion;
    public AudioMixerGroup mixer;
    // Use this for initialization
    void Start () {
        count = 0;
        allowed_to_throw = true;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Shoot(float distance)
    {
        int k = this.gameObject.GetComponent<Additional_power_ups>().bounce_limit * 2;
        if(k==0)
        {
            k = 1;
        }
        count++;
        strength = (float)(distance-1f)/(float)(k);//distance;
        allowed_to_throw = false;
        Vector3 pos = transform.localPosition+new Vector3(0f,0.5f,0f); // getting player position
        switch (gameObject.GetComponent<AI_Movement>().moving_direction)
        {
            case "Front":
                pos += transform.forward;
                break;
            case "Back":
                pos -= transform.forward;
                break;
            case "Left":
                pos -= transform.right;
                break;
            case "Right":
                pos += transform.right;
                break;
        }
        bomb.GetComponent<Transform>().localPosition = pos; // changing bomb location
        switch (gameObject.GetComponent<AI_Movement>().moving_direction)
        {
            case "Front":
                pos += transform.forward * 0.7f;
                break;
            case "Back":
                pos += -transform.forward * 0.7f;
                break;
            case "Left":
                pos += -transform.right * 0.7f;
                break;
            case "Right":
                pos += transform.right * 0.7f;
                break;
        }
        GameObject AI_bomb = Instantiate(bomb); // creating bomb in the scene
        AI_bomb.name = transform.name + "_bomb_" + count; // renaming bomb
        AI_bomb.layer = 12;
        SphereCollider sphereCollider = AI_bomb.AddComponent<SphereCollider>() as SphereCollider; // adding colliders and rigidbody
        sphereCollider.radius = bomb_collision_radius;
        if (transform.GetComponent<Additional_power_ups>().bounce_limit != 0)
        {
            sphereCollider.material = bounce;
        }
        Rigidbody rb = AI_bomb.AddComponent<Rigidbody>();
        rb.freezeRotation = true;
        switch(gameObject.GetComponent<AI_Movement>().moving_direction)
        {
            case "Front":
                rb.AddForce(transform.forward * strength, ForceMode.Impulse);
                break;
            case "Back":
                rb.AddForce(-transform.forward * strength, ForceMode.Impulse);
                break;
            case "Left":
                rb.AddForce(-transform.right * strength, ForceMode.Impulse);
                break;  
            case "Right":
                rb.AddForce(transform.right * strength, ForceMode.Impulse);
                break;
        }
        AI_bomb.AddComponent<Bomb_height_bug_fix>();
        AI_bomb.GetComponent<Bomb_height_bug_fix>().creator = gameObject;
        AI_bomb.AddComponent<Bomb_spawn_collision>();
        AI_bomb.GetComponent<Bomb_spawn_collision>().creator = gameObject;
        AI_bomb.GetComponent<Bomb_spawn_collision>().explosion_vertical = explosion_vertical;
        AI_bomb.GetComponent<Bomb_spawn_collision>().explosion_horizontal = explosion_horizontal;
        AI_bomb.GetComponent<Bomb_spawn_collision>().explosion = explosion;
        AI_bomb.GetComponent<Bomb_spawn_collision>().mixer = mixer;
    }
}
