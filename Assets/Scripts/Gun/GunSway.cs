using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSway : MonoBehaviour
{
    [Header("Sway Rotation")]
    public float intensity;
    public float smooth;

    private Quaternion originRotation;

    private void Start() {
        originRotation = transform.rotation;
    }

    private void Update() {
        UpdateSway();
    }

    private void UpdateSway() 
    {
        
        //Controls
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        //Calculate Rotation
        Quaternion targetAdjustmentX = Quaternion.AngleAxis(-intensity * mouseX, Vector3.up);
        Quaternion targetAdjustmentY = Quaternion.AngleAxis(intensity * mouseY, Vector3.right);
        Quaternion targetRotation = originRotation * targetAdjustmentX * targetAdjustmentY;

        //Rotate towards target rotation
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * smooth);
    }
}
