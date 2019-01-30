using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_Sensitivity_Destroy : MonoBehaviour {

	// Use this for initialization
	void Start () {
            GameObject[] obj = GameObject.FindGameObjectsWithTag("Sensitivity");
            for (int i = 1; i < obj.Length; i++)
            {
                Destroy(obj[i]);
            }
    }
}
