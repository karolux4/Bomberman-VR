using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.WSA.Input;

public class ControllerVisualization : MonoBehaviour
{
    private Vector3 right_position;
    public GameObject obj;
    //private LineRenderer renderer;
    // Start is called before the first frame update
    /*void Start()
    {
        renderer = obj.GetComponent<LineRenderer>();
    }*/

    // Update is called once per frame
    void Update()
    {
        Debug.Log(InputTracking.GetLocalPosition(XRNode.RightHand));
        right_position = InputTracking.GetLocalPosition(XRNode.RightHand);
        Quaternion rotation = InputTracking.GetLocalRotation(XRNode.RightHand);
        obj.transform.localPosition = new Vector3(0f,0f,50f);// -new Vector3(0f,1f,0f);
        obj.transform.localRotation = Quaternion.identity;
        
        //renderer.SetPosition(0, new Vector3(0,0,0));
        //renderer.SetPosition(1, Vector3.forward*100f);
    }
}

