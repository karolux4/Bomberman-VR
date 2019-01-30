using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats_Destroy : MonoBehaviour {

    void Start()
    {
        GameObject[] obj = GameObject.FindGameObjectsWithTag("Stats");
        for (int i = 1; i < obj.Length; i++)
        {
            Destroy(obj[i]);
        }
    }
}
