using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    
    public Transform target;

    float pitch = 0;
    float yaw = 0;

    public float lookSensitivityX = 1;
    public float lookSensitivityY = 1;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    
    void LateUpdate()
    {

        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        yaw += mx * lookSensitivityX;
        pitch += my * lookSensitivityY;

        pitch = Mathf.Clamp(pitch, -80, 80);

        transform.rotation = Quaternion.Euler(pitch, yaw, 0);

        if(target != null){
            //transform.position = AnimMath.Ease(transform.position, target.position, .001f);
            transform.position = target.position;
        }
    }
}
