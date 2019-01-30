using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Shooting_physics : MonoBehaviour {

    public GameObject bomb;
    public GameObject player;
    public GameObject explosion_vertical;
    public GameObject explosion_horizontal;
    public PhysicMaterial bounce;
    public int count { get; set; }
    public bool allowed_to_throw { get; set; }
    public float strength;
    public float bomb_collision_radius;
    public AudioClip explosion;
    public AudioMixerGroup mixer;
    public string Shoot_button;
    private void Start()
    {
        count = 0;
        allowed_to_throw = true;
    }
    // Update is called once per frame
    void Update ()
    {
        bool pressed = Input.GetButtonDown(Shoot_button); //checking if player wants to shoot
        if (pressed && count < gameObject.GetComponent<Additional_power_ups>().limit && allowed_to_throw)
        {
            allowed_to_throw = false;
            count++; // counting amount of player bombs that has not exploded
            Shoot(); // dropping bomb
        }
	}
    void Shoot()
    {
        Vector3 pos = player.GetComponent<Transform>().localPosition+new Vector3(0f,0.5f,0f); // getting player position
        pos += transform.forward*0.7f;
        bomb.GetComponent<Transform>().localPosition = pos; // changing bomb location
        GameObject player_bomb= Instantiate(bomb); // creating bomb in the scene
        player_bomb.name = player.name + "_bomb_"+count; // renaming bomb
        player_bomb.layer = 12;
        SphereCollider sphereCollider = player_bomb.AddComponent<SphereCollider>() as SphereCollider; // adding colliders and rigidbody
        sphereCollider.radius = bomb_collision_radius;
        if (player.GetComponent<Additional_power_ups>().bounce_limit != 0)
        {
            sphereCollider.material = bounce;
        }
        Rigidbody rb = player_bomb.AddComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.AddForce(transform.forward * strength, ForceMode.Impulse);
        player_bomb.AddComponent<Bomb_height_bug_fix>();
        player_bomb.GetComponent<Bomb_height_bug_fix>().creator = player;
        player_bomb.AddComponent<Bomb_spawn_collision>();
        player_bomb.GetComponent<Bomb_spawn_collision>().creator = player;
        player_bomb.GetComponent<Bomb_spawn_collision>().explosion_vertical = explosion_vertical;
        player_bomb.GetComponent<Bomb_spawn_collision>().explosion_horizontal = explosion_horizontal;
        player_bomb.GetComponent<Bomb_spawn_collision>().explosion = explosion;
        player_bomb.GetComponent<Bomb_spawn_collision>().mixer = mixer;
    }
}
