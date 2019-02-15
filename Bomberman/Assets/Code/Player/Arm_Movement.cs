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
            Vector3 Rposition = InputTracking.GetLocalPosition(XRNode.RightHand);
            Vector3 Lposition = InputTracking.GetLocalPosition(XRNode.LeftHand);
            float Langle, Rangle;
            Coordinates(Rposition, out Rangle);
            Coordinates(Lposition, out Langle);
            transform.GetComponent<Arm_Movement>().L_Arm.GetComponent<Transform>().localEulerAngles = new Vector3(Langle, 15f, 90f);
            transform.GetComponent<Arm_Movement>().R_Arm.GetComponent<Transform>().localEulerAngles = new Vector3(Rangle, -15f, -90f);
        }
    }
    private void Coordinates(Vector3 position,out float angle)
    {
        float y1 = position.y - 0.8f;
        if (y1 > 1f)
        {
            y1 = 1f;
        }
        else if (y1 < 0f)
        {
            y1 = 0f;
        }
        y1 = 0.4f + y1 / 5;
        float z1 = Mathf.Sqrt(0.16f - Mathf.Pow((0.6f - y1), 2));
        angle = -90 - (0.44f - (Mathf.Pow(Mathf.Tan(y1 / z1), -1))) / 0.00525f;
    }
}
