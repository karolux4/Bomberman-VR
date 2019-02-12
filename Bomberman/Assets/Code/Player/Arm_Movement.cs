using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.WSA.Input;

public class Arm_Movement : MonoBehaviour
{
    public GameObject L_Arm;
    public GameObject R_Arm;
    private void Update()
    {
        foreach (var sourceState in InteractionManager.GetCurrentReading())
        {
            var sourcePose = sourceState.sourcePose;
            Vector3 position = InputTracking.GetLocalPosition(XRNode.RightHand);
            float y1 = position.y - 1.1f;
            if (y1 > 0.6f)
            {
                y1 = 0.6f;
            }
            else if (y1 < 0.4f)
            {
                y1 = 0.4f;
            }
            float z1 = Mathf.Sqrt(0.16f - Mathf.Pow((0.6f - y1), 2));
            float angle = 145 - (0.44f - (Mathf.Pow(Mathf.Tan(y1 / z1), -1))) / 0.00925f;
            transform.GetComponent<Arm_Movement>().L_Arm.GetComponent<Transform>().localEulerAngles = new Vector3(angle, 0f, -15f);
            transform.GetComponent<Arm_Movement>().R_Arm.GetComponent<Transform>().localEulerAngles = new Vector3(angle, 0f, 15f);
        }
    }
}
