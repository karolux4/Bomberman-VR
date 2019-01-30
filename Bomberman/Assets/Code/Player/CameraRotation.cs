using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour {

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
        player = this.transform.parent.gameObject;
        prev_mouse_pos = new Vector2(player.transform.eulerAngles.y, 0);
	}
	
	// Update is called once per frame
	void Update ()
    {
        sensitivity = GameObject.Find("Sensitivity").GetComponent<Sensitivity_Value>().Sensitivity;
        if ((Time.timeScale != 0)&&(this.gameObject.transform.localPosition.y<3))
        {
            var mouse_delta = new Vector2(Input.GetAxisRaw(MouseX), Input.GetAxisRaw(MouseY));
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
            transform.localRotation = Quaternion.AngleAxis(-prev_mouse_pos.y, Vector3.right);
            player.transform.localRotation = Quaternion.AngleAxis(prev_mouse_pos.x, player.transform.up);
        }
	}
}
