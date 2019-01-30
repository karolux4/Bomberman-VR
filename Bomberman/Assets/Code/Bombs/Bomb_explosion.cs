using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Bomb_explosion : MonoBehaviour {
    public GameObject creator { get; set; }
    public GameObject explosion_vertical { get; set; }
    public GameObject explosion_horizontal { get; set; }
    public bool exploding { get; set; }
    public Coroutine Explosive;
    public AudioClip explosion { get; set; }
    public AudioMixerGroup mixer { get; set; }
    private int audio_count=0;
    // Use this for initialization
    void Start () {
        exploding = false;
        Explosive = StartCoroutine(Explosion());
	}
    IEnumerator Explosion()
    {
        this.gameObject.layer = 11;
        this.gameObject.tag = "Bombs";
        GameObject child = gameObject.transform.GetChild(0).gameObject;
        child.layer = 11;
        child.tag = "Bombs";
        child.name = "Bomb";
        yield return new WaitForSeconds(0.2f);
        if (creator.tag == "Player")
        {
            creator.GetComponent<Shooting_physics>().allowed_to_throw = true;
        }
        else
        {
            creator.GetComponent<AI_Shooting>().allowed_to_throw = true;
        }
        yield return new WaitForSeconds(creator.GetComponent<Additional_power_ups>().explosion_time);
        Explode("No");
        if (creator.tag == "Player")
        {
            creator.GetComponent<Shooting_physics>().count--;
        }
        else
        {
            creator.GetComponent<AI_Shooting>().count--;
        }
        Destroy(this.gameObject);
    }
    public void Sound(Transform t)
    {
        GameObject Audio = new GameObject();
        Audio.transform.localPosition = t.localPosition;
        //Instantiate(Audio);
        Audio.name = "Audio";
        AudioSource audio = Audio.AddComponent<AudioSource>();
        audio.minDistance = 1;
        audio.rolloffMode = AudioRolloffMode.Linear;
        audio.spatialBlend = 1;
        audio.maxDistance = 10;
        audio.outputAudioMixerGroup = mixer;
        audio.PlayOneShot(explosion);
        Audio.AddComponent<AudioDestroy>();
    }
    public void Explode(string message)
    {
        if (audio_count == 0)
        {
            Sound(this.gameObject.GetComponent<Transform>());
            audio_count++;
        }
        float posX, posZ;
        exploding = true;
        CenterPosition(gameObject,out posX,out posZ);
        explosion_vertical.GetComponent<Transform>().localPosition = new Vector3(posX, 1.5f, posZ);
        explosion_horizontal.GetComponent<Transform>().localPosition = new Vector3(posX, 1.5f, posZ);
        var main1 = explosion_vertical.GetComponent<ParticleSystem>().main;
        main1.startSpeed = creator.GetComponent<Additional_power_ups>().bomb_power * 5f;
        var main2 = explosion_horizontal.GetComponent<ParticleSystem>().main;
        main2.startSpeed = creator.GetComponent<Additional_power_ups>().bomb_power * 5f;
        // vertical explosion
        Instantiate(explosion_vertical);
        // horizontal explosion
        Instantiate(explosion_horizontal);
        List<GameObject> hit_players = new List<GameObject>();
        int i = -1;
        ExplosionRays(this.gameObject, creator.GetComponent<Additional_power_ups>().bomb_power,ref i,hit_players);
        if (message == "Yes")
        {
            if (creator.tag == "Player")
            {
                creator.GetComponent<Shooting_physics>().count--;
                creator.GetComponent<Shooting_physics>().allowed_to_throw = true;
            }
            else
            {
                creator.GetComponent<AI_Shooting>().count--;
                creator.GetComponent<AI_Shooting>().allowed_to_throw = true;
            }
            Destroy(gameObject);
        }

    }
    public void ExplosionRays(GameObject obj,float exploding_power,ref int i, List<GameObject> hit_players)
    {
        float posX, posZ;
        CenterPosition(obj, out posX, out posZ);
        Vector3 position = new Vector3(posX, gameObject.transform.localPosition.y, posZ);
        if (i == -1)
        {
            for (i = 0; i < 4; i++)
            {
                SphereCast(position, exploding_power, ref hit_players, i);
            }
        }
        else
        {
            SphereCast(position, exploding_power, ref hit_players, i);
        }
        InitialPositionCollision(posX, posZ, ref hit_players);
    }
    private void SphereCast(Vector3 position, float exploding_power, ref List<GameObject> hit_players,int i)
    {
        RaycastHit Hit;
        int layer_mask = LayerMask.GetMask("Player", "Map", "Bombs", "AI");
        if (Physics.SphereCast(position, 0.48f, Quaternion.Euler(0, i * 90, 0) * transform.forward, out Hit, Mathf.Infinity, layer_mask))
        {
            float distance1 = Hit.distance;
            if (Hit.collider.gameObject.layer != 11)
            {
                if (Hit.collider.GetComponentInParent<BoxCollider>() != null)
                {
                    if ((distance1 < exploding_power) && (Hit.collider.GetComponentInParent<BoxCollider>().tag == "Boxes"))
                    {
                        Destroy(Hit.collider.gameObject.GetComponentInParent<BoxCollider>().gameObject);
                    }
                }
                if ((distance1 < exploding_power) && ((Hit.collider.tag == "Player") || (Hit.collider.tag == "AI")))
                {
                    if (!Hit.collider.gameObject.GetComponent<Additional_power_ups>().invincible)
                    {
                        Hit.collider.gameObject.GetComponent<Additional_power_ups>().lifes_count--;
                        if(Hit.collider.gameObject.GetComponent<Additional_power_ups>().lifes_count<=0 && Hit.collider.name != creator.name)
                        {
                            int index = Index(creator.name);
                            GameObject.Find("Stats").gameObject.GetComponent<Stats>().Kills[index]++;
                        }
                        Hit.collider.gameObject.GetComponent<Additional_power_ups>().invincible = true;
                        Hit.collider.gameObject.GetComponent<Additional_power_ups>().Hit();
                    }
                    hit_players.Add(Hit.collider.gameObject);
                    ExplosionRays(Hit.collider.gameObject, exploding_power - distance1,ref i, hit_players);
                }
            }
            else
            {
                if ((distance1 < exploding_power) && (!Hit.collider.gameObject.GetComponent<Bomb_explosion>().exploding))
                {
                    Hit.collider.gameObject.GetComponent<Bomb_explosion>().StopCoroutine(Hit.collider.gameObject.GetComponent<Bomb_explosion>().Explosive);
                    Hit.collider.gameObject.GetComponent<Bomb_explosion>().Explode("Yes");
                }
            }
        }
    }
    private void CenterPosition(GameObject obj, out float posX, out float posZ)
    {
        Vector3 pos = new Vector3();
        pos = obj.GetComponent<Transform>().localPosition;
        if (pos.x >= 0)
        {
            posX = (int)pos.x + 0.5f;
        }
        else
        {
            posX = Mathf.Sign(pos.x) * (Mathf.Abs((int)pos.x) + 0.5f);
        }
        if (pos.z >= 0)
        {
            posZ = (int)pos.z + 0.5f;
        }
        else
        {
            posZ = Mathf.Sign(pos.z) * ((int)Mathf.Abs(pos.z) + 0.5f);
        }
    }
    private void InitialPositionCollision(float posX, float posZ, ref List<GameObject> hit_players) // needs fix
    {
        RaycastHit hit;
        Vector3 position = new Vector3(posX, 1.2f, posZ);
        int layer_mask = LayerMask.GetMask("Player", "Map", "Bombs", "AI");
        if(Physics.Raycast(position+new Vector3(0f,2f,0f),transform.up*(-1),out hit, 3f, layer_mask))
        {
            if (hit.collider.tag == "Player" || hit.collider.tag == "AI")
            {
                if (!hit_players.Contains(hit.collider.gameObject))
                {
                    if (!hit.collider.gameObject.GetComponent<Additional_power_ups>().invincible)
                    {
                        hit.collider.gameObject.GetComponent<Additional_power_ups>().lifes_count--;
                        if (hit.collider.gameObject.GetComponent<Additional_power_ups>().lifes_count <= 0 && hit.collider.name != creator.name)
                        {
                            int index = Index(creator.name);
                            GameObject.Find("Stats").gameObject.GetComponent<Stats>().Kills[index]++;
                        }
                        hit.collider.gameObject.GetComponent<Additional_power_ups>().invincible = true;
                        hit.collider.gameObject.GetComponent<Additional_power_ups>().Hit();
                    }
                    hit_players.Add(hit.collider.gameObject);
                }
            }
            else if (hit.collider.tag == "Bombs")
            {
                if (!hit.collider.gameObject.GetComponent<Bomb_explosion>().exploding)
                {
                    hit.collider.gameObject.GetComponent<Bomb_explosion>().StopCoroutine(hit.collider.gameObject.GetComponent<Bomb_explosion>().Explosive);
                    hit.collider.gameObject.GetComponent<Bomb_explosion>().Explode("Yes");
                }
            }
        }
        for (int i=0;i<8;i++)
        {
            if (Physics.Raycast(position, Quaternion.Euler(0, i * 45, 0) * transform.forward, out hit, 0.6f, layer_mask))
            {
                if(hit.collider.tag=="Player"||hit.collider.tag=="AI")
                {
                    if (!hit_players.Contains(hit.collider.gameObject))
                    {
                        if (!hit.collider.gameObject.GetComponent<Additional_power_ups>().invincible)
                        {
                            hit.collider.gameObject.GetComponent<Additional_power_ups>().lifes_count--;
                            if (hit.collider.gameObject.GetComponent<Additional_power_ups>().lifes_count <= 0 && hit.collider.name!=creator.name)
                            {
                                int index = Index(creator.name);
                                GameObject.Find("Stats").gameObject.GetComponent<Stats>().Kills[index]++;
                            }
                            hit.collider.gameObject.GetComponent<Additional_power_ups>().invincible = true;
                            hit.collider.gameObject.GetComponent<Additional_power_ups>().Hit();
                        }
                        hit_players.Add(hit.collider.gameObject);
                    }
                }
                else if(hit.collider.tag=="Bombs")
                {
                    if (!hit.collider.gameObject.GetComponent<Bomb_explosion>().exploding)
                    {
                        hit.collider.gameObject.GetComponent<Bomb_explosion>().StopCoroutine(hit.collider.gameObject.GetComponent<Bomb_explosion>().Explosive);
                        hit.collider.gameObject.GetComponent<Bomb_explosion>().Explode("Yes");
                    }
                }
            }
        }
    }
    private int Index(string name)
    {
        int index = 0;
        if (name[name.Length - 1] == '4' || name[name.Length - 1] == '3' || name[name.Length - 1] == '2' || name[name.Length - 1] == '1')
        {
            index = int.Parse(name[name.Length - 1].ToString())-1;
        }
        else
        {
            index = 0;
        }
        return index;
    }
}
