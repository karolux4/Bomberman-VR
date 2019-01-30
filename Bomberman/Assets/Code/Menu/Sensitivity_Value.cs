using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensitivity_Value : MonoBehaviour {

    public float Sensitivity;
	// Use this for initialization
	void Start () {
        Sensitivity = 3;
        DontDestroyOnLoad(this.gameObject);
	}
}
