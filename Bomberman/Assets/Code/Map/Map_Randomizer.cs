using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map_Randomizer : MonoBehaviour {
    private int count = 0;
    // Use this for initialization
    void Start () {
        Randomizer();
        if(count<60)
        {
            Randomizer();
        }
	}
    void Randomizer()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if(!transform.GetChild(i).gameObject.activeSelf)
            {
                if(Random.Range(0,100)%2==0)
                {
                    transform.GetChild(i).gameObject.SetActive(true);
                    count++;
                }
            }
        }
    }
}
