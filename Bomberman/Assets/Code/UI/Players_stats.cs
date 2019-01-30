using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Players_stats : MonoBehaviour {

    private GameObject Stats;
    public List<GameObject> Players;
    // Update is called once per frame
    private void Start()
    {
        Stats = GameObject.Find("Stats");
    }
    void Update () {
        for(int i=1;i<5;i++)
        {
            GameObject.Find("PlayerName" + i).gameObject.GetComponent<TextMeshProUGUI>().text = Players[i-1].name;
            GameObject.Find("Kills" + i).gameObject.GetComponent<TextMeshProUGUI>().text = Stats.GetComponent<Stats>().Kills[i-1].ToString();
            GameObject.Find("Wins" + i).gameObject.GetComponent<TextMeshProUGUI>().text = Stats.GetComponent<Stats>().Wins[i-1].ToString();
            GameObject.Find("Deaths" + i).gameObject.GetComponent<TextMeshProUGUI>().text = Stats.GetComponent<Stats>().Deaths[i-1].ToString();
        }
	}
}
