using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class CameraRotation : MonoBehaviour {

    public GameObject Main_Camera_Control;
    public Vector2 prev_mouse_pos;
    Vector2 smooth;
    public float sensitivity;
    public float smoothing;
    GameObject player;
    public string MouseX;
    public string MouseY;
	// Use this for initialization
	void Start ()
    {
        player = this.transform.parent.gameObject.transform.parent.gameObject;
        prev_mouse_pos = new Vector2(player.transform.eulerAngles.y, 0);
	}
	
	// Update is called once per frame
	void Update ()
    {
        /*sensitivity = GameObject.Find("Sensitivity").GetComponent<Sensitivity_Value>().Sensitivity;
        if ((Time.timeScale != 0)&&(Main_Camera_Control.transform.localPosition.y<3))
        {
            var mouse_delta = new Vector2(Input.GetAxisRaw(MouseX), 0f);
            mouse_delta = Vector2.Scale(mouse_delta, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
            smooth.x = Mathf.Lerp(smooth.x, mouse_delta.x, 1f / smoothing);
            smooth.y = Mathf.Lerp(smooth.y, mouse_delta.y, 1f / smoothing);
            prev_mouse_pos += smooth;
            if (prev_mouse_pos.y>17)
            {
                prev_mouse_pos.y = 17;
            }
            if(prev_mouse_pos.y<-75)
            {
                prev_mouse_pos.y = -75;
            }
            Main_Camera_Control.transform.localRotation = Quaternion.AngleAxis(-prev_mouse_pos.y, Vector3.right);
            player.transform.localRotation = Quaternion.AngleAxis(prev_mouse_pos.x, player.transform.up);
        }*/
        float y = (float)this.gameObject.transform.localRotation.eulerAngles.y; //* (float)3)/(float)2;
        if(y>180)
        {
            y = -360 +(y-((int)y/(int)360)*360);
        }
        //float y1 = (float)(y*3)/(float)2;
        //Debug.Log(y+" "+y1);
        player.transform.localRotation = Quaternion.Euler(0f,y , 0f);
        Main_Camera_Control.transform.localRotation = Quaternion.Euler(0f, -this.gameObject.transform.localRotation.eulerAngles.y, 0f);
	}
}
