using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootRaycast : MonoBehaviour {
    
    public float raycastLength = 2;
    private float distanceBetweenGroundAndIK = 0;

    private Quaternion startingRot;

    void Start() {
        startingRot = transform.localRotation;
        distanceBetweenGroundAndIK = transform.localPosition.y;
    }

    void Update() {
        

        Vector3 origin = transform.position + Vector3.up * raycastLength/2;
        Vector3 direction = Vector3.down;

        // draw the ray in the scene:
        Debug.DrawRay(origin, direction * raycastLength, Color.blue);

        // check for collision with ray:
        if(Physics.Raycast(origin, direction, out RaycastHit hitInfo, raycastLength)){

            // finds ground position
            transform.position = hitInfo.point + Vector3.up * distanceBetweenGroundAndIK;

            // convert starting rotation into world-space
            Quaternion worldNeutral = transform.parent.rotation * startingRot;

            // finds ground rotation
            transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal) * worldNeutral;

        }

    }
}
