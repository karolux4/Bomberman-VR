using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Power_Ups : MonoBehaviour {
    public GameObject Player, AI_1, AI_2, AI_3;
    public GameObject Bomb_kick;
    public GameObject Damage;
    public GameObject Limit;
    public GameObject Speed;
    public GameObject Bounce;
    public GameObject AI_1_Image;
    public GameObject AI_2_Image;
    public GameObject AI_3_Image;
	void Update ()
    {
		if(Player.GetComponent<Additional_power_ups>().bomb_kick)
        {
            Bomb_kick.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
        Bounce.GetComponent<TextMeshProUGUI>().text = (Player.GetComponent<Additional_power_ups>().bounce_limit).ToString();
        Damage.GetComponent<TextMeshProUGUI>().text = (Player.GetComponent<Additional_power_ups>().bomb_power).ToString();
        Limit.GetComponent<TextMeshProUGUI>().text = (Player.GetComponent<Additional_power_ups>().limit).ToString();
        Speed.GetComponent<TextMeshProUGUI>().text = ((float)(Player.GetComponent<Additional_power_ups>().speed-0.5)/(float)0.5).ToString();
        if(!AI_1.activeInHierarchy)
        {
            AI_1_Image.GetComponent<Image>().color = new Color32(135, 135, 135, 135);
        }
        if (!AI_2.activeInHierarchy)
        {
            AI_2_Image.GetComponent<Image>().color = new Color32(135, 135, 135, 135);
        }
        if (!AI_3.activeInHierarchy)
        {
            AI_3_Image.GetComponent<Image>().color = new Color32(135, 135, 135, 135);
        }
    }
}
