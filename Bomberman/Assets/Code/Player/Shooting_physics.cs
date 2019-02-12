using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.WSA.Input;

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
    private bool released = true;

    public GameObject[] ThrowingAssets;
    public InteractionSourceNode ControllerPose = InteractionSourceNode.Grip;
    public Transform RealWorldRoot;

    private readonly Dictionary<uint, Transform> devices = new Dictionary<uint, Transform>();
    private readonly Dictionary<uint, int> modelIndecies = new Dictionary<uint, int>();
    private readonly Dictionary<uint, bool> isDetatched = new Dictionary<uint, bool>();

    private void Start()
    {
        count = 0;
        allowed_to_throw = true;
        InteractionManager.InteractionSourceReleased += InteractionManager_InteractionSourceReleased;
        InteractionManager.InteractionSourcePressed += InteractionManager_InteractionSourcePressed;
        Application.onBeforeRender += Application_onBeforeRender;
    }
    private void Update()
    {
        /*foreach(var dev in devices)
        {
            Debug.Log(dev.Value);
        }*/
       // Debug.Log(count + " " + allowed_to_throw);
    }
    private void InteractionManager_InteractionSourcePressed(InteractionSourcePressedEventArgs args)
    {
        //Debug.Log(count);
        if ((count < player.GetComponent<Additional_power_ups>().limit) && (allowed_to_throw))
        {
            uint id = args.state.source.id;
            if (args.pressType == InteractionSourcePressType.Select)
            {
                if (isDetatched.ContainsKey(id))
                {
                    isDetatched[id] = false;
                }
                AddDevice(id);
            }
        }
    }

    private void InteractionManager_InteractionSourceReleased(InteractionSourceReleasedEventArgs args)
    {
        if (args.pressType == InteractionSourcePressType.Select)
        {
            uint id = args.state.source.id;
            if (devices.ContainsKey(id))
            {
                var go = devices[id];
                var rigidbody = go.GetComponent<Rigidbody>();
                if (rigidbody == null)
                {
                    rigidbody = go.GetComponentInChildren<Rigidbody>();
                }
                if (rigidbody.TryThrow(args.state.sourcePose,player))
                {
                    DetatchDevice(id);
                }
                else
                {
                    count--;
                    allowed_to_throw = true;
                    RemoveDevice(id);
                    throw new System.Exception("Throw failed!!!");
                }
            }
        }
    }

    private void Application_onBeforeRender()
    {
        foreach (var sourceState in InteractionManager.GetCurrentReading())
        {
            uint id = sourceState.source.id;
            var handedness = sourceState.source.handedness;
            var sourcePose = sourceState.sourcePose;
            Vector3 position;
            Quaternion rotation;
            if (devices.ContainsKey(id))
            {
                if (sourcePose.TryGetPosition(out position, this.ControllerPose) &&
                    sourcePose.TryGetRotation(out rotation, this.ControllerPose)) // defaults to grip
                {
                    //rotation = Quaternion.Euler(0f, 0f, 0f);
                    SetTransform(devices[id], position, rotation);
                }
            }
            /*else if (sourceState.source.supportsPointing)
            {
                if (this.modelIndecies.ContainsKey(id))
                {
                    Debug.Log("Add1");
                    this.AddDevice(id, this.modelIndecies[id]);
                }
                else
                {
                    Debug.Log("Add2");
                    this.AddDevice(id);
                }

                if (!isDetatched.ContainsKey(id) || !isDetatched[id])
                {
                    if (sourcePose.TryGetPosition(out position, this.ControllerPose) &&
                    sourcePose.TryGetRotation(out rotation, this.ControllerPose)) // defaults to grip
                    {
                        //rotation = Quaternion.Euler(0f, 0f, 0f);
                        SetTransform(devices[id], position, rotation);
                    }
                }
            }*/
        }
    }
    private void AddDevice(uint id, int index = 0)
    {
        if (!devices.ContainsKey(id) && (!isDetatched.ContainsKey(id) || !isDetatched[id]))
        {
            count++;
            allowed_to_throw = false;
            GameObject go = Instantiate(this.ThrowingAssets[index], player.transform);
            go.name = player.name + "_bomb";
            devices[id] = go.transform;
            modelIndecies[id] = index;
            isDetatched[id] = false;
        }
    }

    private void RemoveDevice(uint id)
    {
        if (devices.ContainsKey(id))
        {
            Destroy(devices[id].gameObject);
            devices.Remove(id);
        }
    }

    private void DetatchDevice(uint id)
    {
        if (devices.ContainsKey(id))
        {
            devices[id].SetParent(null);
            Shoot(devices[id].gameObject);
            isDetatched[id] = true;
            devices.Remove(id);
        }
    }

    private void SetTransform(Transform t, Vector3 position, Quaternion rotation)
    {
        t.localPosition = new Vector3(0f, 0.75f, 0.4f);// +transform.forward*0.7f;
        //Debug.Log(t.localPosition);
        t.localRotation = Quaternion.Euler(0f,0f,0f);
    }

    void OnDestroy()
    {
        InteractionManager.InteractionSourceReleased -= InteractionManager_InteractionSourceReleased;
        InteractionManager.InteractionSourcePressed -= InteractionManager_InteractionSourcePressed;
        Application.onBeforeRender -= Application_onBeforeRender;
    }
    // Update is called once per frame
    /*void Update ()
    {
        bool pressed = Input.GetButtonDown(Shoot_button); //checking if player wants to shoot
        if (pressed && released&& count < gameObject.GetComponent<Additional_power_ups>().limit && allowed_to_throw)
        {
            allowed_to_throw = false;
            released = false;
            count++; // counting amount of player bombs that has not exploded
            Shoot(); // dropping bomb
        }
	}*/
    void Shoot(GameObject player_bomb)
    {
        /*Vector3 pos = player.GetComponent<Transform>().localPosition+new Vector3(0f,0.5f,0f); // getting player position
        pos += transform.forward*0.7f;
        bomb.GetComponent<Transform>().localPosition = pos; // changing bomb location
        GameObject player_bomb= Instantiate(bomb); // creating bomb in the scene
        player_bomb.name = player.name + "_bomb_"+count; // renaming bomb*/
        player_bomb.layer = 12;
        /*
            SphereCollider sphereCollider = player_bomb.AddComponent<SphereCollider>() as SphereCollider; // adding colliders and rigidbody
            sphereCollider.radius = bomb_collision_radius;*/
            if (player.GetComponent<Additional_power_ups>().bounce_limit != 0)
            {
                player_bomb.GetComponent<SphereCollider>().material = bounce;
            }
            /*Rigidbody rb = player_bomb.AddComponent<Rigidbody>();
            rb.freezeRotation = true;
            rb.AddForce(transform.forward * strength, ForceMode.Impulse);*/
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
