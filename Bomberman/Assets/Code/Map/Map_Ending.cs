using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map_Ending : MonoBehaviour {

    public bool game_type; // false - singleplayer, true - multiplayer
    public GameObject Game_End_Box;
    public GameObject Game_Load;
    public float time_limit;
    public Material sky;
    private float timer;
    public bool end_started = false;
	// Use this for initialization
	void Start () {
        timer = 0f;
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if(timer>=time_limit&&!end_started)
        {
            end_started = true;
            RenderSettings.skybox = sky;
            StartCoroutine(Map_End(-5.5f, 8.5f,1,0)); // Start coordinates
        }
	}

    IEnumerator Map_End(float positionX, float positionZ, int signX, int signZ) // Box drop coordinates
    {
        if(!game_type)
        {
            if(Game_Load.GetComponent<Game_Load>().ActiveAICount==0||(!Game_Load.GetComponent<Game_Load>().Player.activeInHierarchy))
            {
                yield return 0;
            }
            else
            {
                Game_End_Box.transform.localPosition = new Vector3(positionX, 3f, positionZ);
                GameObject box = Instantiate(Game_End_Box);
                Place_Destroy(box);
                Position(ref positionX, ref positionZ, ref signX, ref signZ);
                yield return new WaitForSeconds(1);
                StartCoroutine(Map_End(positionX, positionZ, signX, signZ));
            }
        }
        else
        {
            if (Game_Load.GetComponent<Multiplayer_Game_Load>().ActivePlayers_Count == 0 ||
                Game_Load.GetComponent<Multiplayer_Game_Load>().ActiveAI_Count+Game_Load.GetComponent<Multiplayer_Game_Load>().ActivePlayers_Count==1)
            {
                yield return 0;
            }
            else
            {
                Game_End_Box.transform.localPosition = new Vector3(positionX, 3f, positionZ);
                GameObject box = Instantiate(Game_End_Box);
                Place_Destroy(box);
                Position(ref positionX, ref positionZ, ref signX, ref signZ);
                yield return new WaitForSeconds(1);
                StartCoroutine(Map_End(positionX, positionZ, signX, signZ));
            }
        }
    }
    private void Place_Destroy(GameObject box)
    {
        RaycastHit Hit;
        int layer_mask = LayerMask.GetMask("Player", "Map", "Bombs", "AI", "Power_ups");
        if (Physics.SphereCast(box.transform.position, 0.48f, transform.up * -1,out Hit, Mathf.Infinity,layer_mask))
        {
            if(Hit.collider.tag=="Player"||(Hit.collider.tag=="AI"))
            {
                Hit.collider.GetComponent<Additional_power_ups>().lifes_count = -1;
            }
            else if(Hit.collider.gameObject.name!="Ground")
            {
                Destroy(Hit.collider.GetComponentInParent<Transform>().gameObject);
            }
        }
    }
    private float up_X = -5.5f, down_X = 6.5f;
    private float up_Z = 8.5f, down_Z = -3.5f;
    private void Position(ref float positionX, ref float positionZ, ref int signX, ref int signZ)
    {
        if((positionZ==up_Z)&&(positionX!=down_X))
        {
            signX = 1;
            signZ = 0;
        }
        else if((positionZ==down_Z)&&(positionX != up_X))
        {
            signX = -1;
            signZ = 0;
        }

        if((positionX == down_X)&&(positionZ != down_Z))
        {
            signZ = -1;
            signX = 0;
        }
        else if((positionX==up_X)&&(positionZ != up_Z))
        {
            signZ = 1;
            signX = 0;
        }

        if ((positionX == up_X) && (positionZ == (up_Z - 1f)))
        {
            up_X += 1;
            up_Z -= 1;
            down_X -= 1;
            down_Z += 1;
            positionX = up_X;
            positionZ = up_Z;
        }
        else
        {
            positionX = positionX + 1f * signX;
            positionZ = positionZ + 1f * signZ;
        }
    }
}
