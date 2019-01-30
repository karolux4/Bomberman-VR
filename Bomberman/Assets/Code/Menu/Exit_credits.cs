using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit_credits : MonoBehaviour {
    public GameObject Credits;
    public GameObject Main_Menu;
	// Update is called once per frame
	void Update () {
		if(Input.GetButton("Cancel"))
        {
            Credits.SetActive(false);
            Main_Menu.SetActive(true);
        }
	}
}
