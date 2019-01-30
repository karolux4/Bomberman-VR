using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power_up_randomizer : MonoBehaviour {

    public GameObject Speed; //0
    public GameObject Bounce; //1
    public GameObject Limit; //2
    public GameObject Heart; //3
    public GameObject Kick; //4
    public GameObject Damage; //5
    public RuntimeAnimatorController Animations;
    private List<GameObject> Power_ups;
    public GameObject MapEnding;
    void Start() {
        Power_up_creation();
    }

    // Update is called once per frame
    public float timer = 0.0f;
    void Update() {
        if(!this.gameObject.GetComponent<BoxCollider>().enabled)
        {
            timer += Time.deltaTime;
        }
        if(timer>=90&&!MapEnding.GetComponent<Map_Ending>().end_started)
        {
            Power_up_creation();
            timer = 0;
            this.gameObject.GetComponent<BoxCollider>().enabled = true;
        }
    }
    private string Power_up_tag(GameObject power_up)
    {
        string tag = "";
        if (power_up.name == "Boots(Clone)")
        {
            tag = "Speed_power_up";
        }
        else if(power_up.name=="Kick(Clone)")
        {
            tag = "Bomb_kick_power_up";
        }
        else if (power_up.name == "Heart(Clone)")
        {
            tag = "Extra_life_power_up";
        }
        else if (power_up.name == "BouncePowerUp(Clone)")
        {
            tag = "Bounce_power_up";
        }
        else if (power_up.name == "CPU(Clone)")
        {
            tag = "Limit_power_up";
        }
        else
        {
            tag = "Strength_power_up";
        }
        return tag;
    }
    void Power_up_creation()
    {
        Power_ups = new List<GameObject>();
        Power_ups.Add(Speed);
        Power_ups.Add(Bounce);
        Power_ups.Add(Limit);
        Power_ups.Add(Heart);
        Power_ups.Add(Kick);
        Power_ups.Add(Damage);
        int index = Random.Range(0, 60);
        index = index % 6;
        GameObject Power_up = Instantiate(Power_ups[index], this.gameObject.transform);
        Power_up.GetComponent<Animator>().runtimeAnimatorController = Animations;
        Power_up.transform.SetParent(this.gameObject.GetComponent<Transform>());
        string tag = Power_up_tag(Power_up);
        this.gameObject.tag = tag;
    }
}
