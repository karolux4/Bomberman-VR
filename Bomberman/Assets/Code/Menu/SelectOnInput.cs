using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectOnInput : MonoBehaviour {

    public EventSystem eventSystem;
    public GameObject obj;
    private bool button_selected;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if((Input.GetAxisRaw("Vertical")!=0||Input.GetAxisRaw("Horizontal")!=0)&&!button_selected)
        {
            eventSystem.SetSelectedGameObject(obj);
            button_selected = true;
        }
	}
    private void OnDisable()
    {
        button_selected = false;
    }
}
